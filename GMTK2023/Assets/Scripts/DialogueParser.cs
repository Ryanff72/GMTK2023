using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;

public static class DialogueParser
{
    public static string Dialogue;

    public static void initDialogue()
    {
        var fileStream = new FileStream(Application.dataPath + "/Dialogue.txt", FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            Dialogue = streamReader.ReadToEnd();
        }
    }
    public static List<string> LoadDialogue(string Name, int Hp, int Day, string LastPotion)
    {
        int CharacterDlgIndex = Dialogue.IndexOf("__" + Name) + Name.Length + 6;
        string CharacterStr = "";

        for (int i = CharacterDlgIndex; i < Dialogue.Length; i++)
        {
            if (i + 1 < Dialogue.Length && Dialogue[i] == '_' && Dialogue[i + 1] == '_')
            {
                break;
            }
            CharacterStr += Dialogue[i];
        }
        CharacterStr = CharacterStr.Replace("    ", "");
        CharacterStr = CharacterStr.Replace("\t", "");

        List<string> CharacterStrSplit = CharacterStr.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        List<string> loadedDialogue = new List<string>();
        bool skipping = true;
        bool foundDay = false;
        bool skippingDay = false;

        for (int i = 0; i < CharacterStrSplit.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(CharacterStrSplit[i]) || CharacterStrSplit[i][0] == '\n')
            {
                continue;
            }
            if (CharacterStrSplit[i][0] == '_')
            {

                switch (CharacterStrSplit[i].Substring(0, 4))
                {
                    case "_day":
                        if (foundDay)
                        {
                            return loadedDialogue;
                        }
                        if (CharacterStrSplit[i][5] - '0' != Day)
                        {
                            skipping = true;
                            skippingDay = true;
                            continue;
                        }
                        else
                        {
                            skipping = false;
                            foundDay = true;
                            skippingDay = false;

                            continue;
                        }
                    case "_hp ":
                        if (skippingDay)
                        {
                            continue;
                        }
                        if (CharacterStrSplit[i][4] - '0' != Hp)
                        {
                            skipping = true;
                            continue;
                        }
                        else
                        {
                            skipping = false;
                            continue;
                        }
                    case "_awf":
                        if (skippingDay)
                        {
                            continue;
                        }
                        if (LastPotion != "awf")
                        {
                            skipping = true;
                            continue;
                        }
                        else
                        {
                            skipping = false;
                            continue;
                        }
                    case "_mid":
                        if (skippingDay)
                        {
                            continue;
                        }
                        if (LastPotion != "mid")
                        {
                            skipping = true;
                            continue;
                        }
                        else
                        {
                            skipping = false;
                            continue;
                        }
                    case "_god":
                        if (skippingDay)
                        {
                            continue;
                        }
                        if (LastPotion != "god")
                        {
                            skipping = true;
                            continue;
                        }
                        else
                        {
                            skipping = false;
                            continue;
                        }
                    case "_itm":
                        if (!skipping)
                        {
                            return loadedDialogue;
                        }
                        break;
                    default:
                        if (skippingDay)
                        {
                            continue;
                        }
                        skipping = false;
                        continue;
                }
            }
            if (!skipping)
            {
                loadedDialogue.Add(CharacterStrSplit[i]);
            }
        }
        return loadedDialogue;
    }
}