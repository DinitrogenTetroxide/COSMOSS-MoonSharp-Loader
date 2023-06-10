﻿using ModLoader;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using NLua;
using ModLoader.Helpers;
using UITools;
using SFS.IO;
using HarmonyLib;

namespace LuaInterpreter
{
    public class Main : Mod, IUpdatable
    {
        public override string ModNameID => "LuaInterpreter";
        public override string DisplayName => "AstroForge";
        public override string Author => "N2O4";
        public override string MinimumGameVersionNecessary => "1.5.10.2";
        public override string ModVersion => "1.0";
        public override string Description => "A very simple Lua (NLua) script loader.";
        public override string IconLink => "https://i.imgur.com/JCK73S8.png";

        public Dictionary<string, FilePath> UpdatableFiles => new Dictionary<string, FilePath>
        {{ 
            "https://github.com/DinitrogenTetroxide/AstroForge/releases/latest/download/LuaInterpreter.dll",
            new FolderPath(ModFolder).ExtendToFile("LuaInterpreter.dll") 
        }};

        public string InterpreterPath;
        public static string dllDirectory;

        public Harmony patcher;

        public ScriptLoader loader;

        // Basically Monobehaviour for the poor people

        public override void Early_Load()
        {
            LoadInterpreter();
        }
        void OnLog(string msg, string st, LogType lt) 
        {
            TextWriter tw = new StreamWriter($"{dllDirectory}/Mods/LuaInterpreter/Logs/latest.log", true);
            tw.Write("TIME_SINCE_START: [" + Time.realtimeSinceStartup + "],\nMSG: " + msg + (st != null && st != "" ? ("STACK_TRACE: " + st + "\n\n") : "\n\n"));
            tw.Close();
        }

        void LoadInterpreter()
        {
            dllDirectory = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/");

            File.Open($"{dllDirectory}/Mods/LuaInterpreter/Logs/latest.log", FileMode.Open).SetLength(0);
            Application.logMessageReceived += OnLog;

            // TODO: Change the engine
            InterpreterPath =
                $"{dllDirectory}/Mods/LuaInterpreter/DLLs/NLua.dll";
            try
            {
                Assembly.LoadFile($"{dllDirectory}/Mods/LuaInterpreter/DLLs/KeraLua.dll");
                Assembly.LoadFile(InterpreterPath);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.Log("[LuaInterpreter] Path: " + InterpreterPath);
            }
        }

        public override void Load()
        {
            loader = new ScriptLoader();
            loader.Load();
        }
    }
}