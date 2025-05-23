// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Kitchen.Components;
using Content.Shared._Shitmed.Medical.Surgery.Tools;
using Robust.Shared.Audio;

namespace Content.Server._Shitmed.Medical.Surgery;

/// <summary>
/// Makes all sharp things usable for incisions and sawing through bones, though worse than any other kind of ghetto analogue.
/// </summary>
public sealed partial class GhettoSurgerySystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SharpComponent, MapInitEvent>(OnSharpInit);
        SubscribeLocalEvent<SharpComponent, ComponentShutdown>(OnSharpShutdown);
    }

    private void OnSharpInit(Entity<SharpComponent> ent, ref MapInitEvent args)
    {
        if (EnsureComp<SurgeryToolComponent>(ent, out var tool))
        {
            ent.Comp.HadSurgeryTool = true;
        }
        else
        {
            tool.StartSound = new SoundPathSpecifier("/Audio/_Shitmed/Medical/Surgery/scalpel1.ogg");
            tool.EndSound = new SoundPathSpecifier("/Audio/_Shitmed/Medical/Surgery/scalpel2.ogg");
            Dirty(ent.Owner, tool);
        }

        if (EnsureComp<ScalpelComponent>(ent, out var scalpel))
        {
            ent.Comp.HadScalpel = true;
        }
        else
        {
            scalpel.Speed = 0.3f;
            Dirty(ent.Owner, scalpel);
        }

        if (EnsureComp<BoneSawComponent>(ent, out var saw))
        {
            ent.Comp.HadBoneSaw = true;
        }
        else
        {
            saw.Speed = 0.2f;
            Dirty(ent.Owner, saw);
        }
    }

    private void OnSharpShutdown(Entity<SharpComponent> ent, ref ComponentShutdown args)
    {
        if (!ent.Comp.HadSurgeryTool)
            RemComp<SurgeryToolComponent>(ent);

        if (!ent.Comp.HadScalpel)
            RemComp<ScalpelComponent>(ent);

        if (!ent.Comp.HadBoneSaw)
            RemComp<BoneSawComponent>(ent);
    }
}