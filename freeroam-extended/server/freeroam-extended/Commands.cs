﻿using System;
using System.Collections.Generic;
using System.Linq;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Resources.Chat.Api;
using Freeroam_Extended.Factories;

namespace Freeroam_Extended
{
    public class Commands : IScript
    {
        [Command("veh")]
        public void SpawnVeh(AltPlayer player, string vehicleName)
        {
            if (player.IsInVehicle)
            {
                player.SendChatMessage("{FF0000} You are already in a vehicle!");
                return;
            }
            
            if (Alt.GetAllVehicles().Any(veh => veh.Position.Distance(player.Position) < 3))
            {
                player.SendChatMessage("{FF0000} You are too close to a vehicle!");
                return;
            }
            
            if (player.LastVehicleSpawn.AddSeconds(10) > DateTime.Now)
            {
                player.SendChatMessage("{FF0000} You have to wait 10s before spawning a new vehicle!");
                return;
            }
            
            if (player.Vehicles.Count >= 3)
            {
                player.Vehicles.OrderBy(veh => veh.SpawnTime).First().Remove();
                return;
            }
            
            var spawnedVeh = (AltVehicle)Alt.CreateVehicle(Alt.Hash(vehicleName), player.Position + new Position(1, 0,0), Rotation.Zero);
            player.SetIntoVehicle(spawnedVeh, 0);
            player.LastVehicleSpawn = DateTime.Now;
        }
        
        [Command("revive")]
        public void Revive(IPlayer player)
        {
            if (player.Health >= 100)
            {
                player.SendChatMessage("{FF0000} You are already alive!");
                return;
            }
            player.Spawn(player.Position);
            player.SendChatMessage("{00FF00} You have been revived!");
        }

        [Command("ghost")]
        public void Ghost(IPlayer player)
        {
            // Disable
            if (player.HasMetaData("GHOST"))
            {
                player.DeleteMetaData("GHOST");
                return;    
            }
            // Enable
            player.SetMetaData("GHOST", true);
            player.Invincible = true;
        }
        
    }
}