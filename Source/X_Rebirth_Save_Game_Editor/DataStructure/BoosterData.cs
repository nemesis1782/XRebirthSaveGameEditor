﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using X_Rebirth_Save_Game_Editor.Helper;
using X_Rebirth_Save_Game_Editor.Logging;

namespace X_Rebirth_Save_Game_Editor.DataStructure
{
    public class BoosterData
    {
        // Used in:
        // Universe -> factions -> faction -> relations: Implemented.
        // Universe -> factions -> faction -> relations: Compatible, not implemented.
        // component class="npc" -> tolerance: Not compatible, missing keywords "delay" and "decayrate" for creation.
        // component class="computer" -> tolerance: Not compatible, missing keywords "delay" and "decayrate" for creation.
        #region Members
        XmlNode BoosterNode = null;
        CatDatExtractor cde = null;
        #endregion

        #region Constructors
        public BoosterData(XmlNode relationNode, CatDatExtractor cde)
        {
            BoosterNode = relationNode;
            this.cde = cde;
        }

        public BoosterData(string faction, float relation, double time, XmlNode parent, CatDatExtractor cde)
        {
            BoosterNode = parent.OwnerDocument.CreateElement("booster");
            BoosterNode.Attributes.Append(parent.OwnerDocument.CreateAttribute("faction"));
            BoosterNode.Attributes["faction"].Value = faction;
            BoosterNode.Attributes.Append(parent.OwnerDocument.CreateAttribute("relation"));
            BoosterNode.Attributes["relation"].Value = XmlConvert.ToString(relation);
            // Max/min value observed in game save
            // 0.15 for a discount booster
            // 0.578817/-1  for a relation booster
            BoosterNode.Attributes.Append(parent.OwnerDocument.CreateAttribute("time"));
            BoosterNode.Attributes["time"].Value = XmlConvert.ToString(time);
            // Usual time in savegame: ~150e3 (not expressed in scientific notation)
            parent.AppendChild(BoosterNode);
        }
        #endregion

        #region Properties
        public string faction
        {
            get
            {
                return XMLFunctions.GetSafeAttribute(BoosterNode, "faction");
            }
        }

        public float Relation
        {
            get
            {
                float f = XmlConvert.ToSingle(XMLFunctions.GetSafeAttribute(BoosterNode, "relation"));
                // Using XmlConvert.ToSingle to convert with the decimal separator used in xml document (dot)
                return f;
            }
            set
            {
                XMLFunctions.SetSafeAttribute(BoosterNode, "relation", XmlConvert.ToString(value));
                // Using XmlConvert.ToString to convert the value with the dot decimal separator whatever was set in the GUI
            }
        }

        public double Time
        {
            get
            {
                double f = XmlConvert.ToDouble(XMLFunctions.GetSafeAttribute(BoosterNode, "time"));
                return f;
            }
            set
            {
                XMLFunctions.SetSafeAttribute(BoosterNode, "time", XmlConvert.ToString(value));
            }
        }
        #endregion

        public void Remove()
        {
            BoosterNode.ParentNode.RemoveChild(BoosterNode);
        }

    }
}