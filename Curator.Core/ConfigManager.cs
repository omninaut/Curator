﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Curator.Core
{
    /// <summary>
    /// Another singleton class. Manages configuration settings in memory, and saving/loading from text file in application directory.
    /// </summary>
        public class ConfigManager : Curator.Utils.IConfigManager
        {
            private static ConfigManager _configManagerInstance;
            private static readonly object _configManagerInstanceSync = new object(); // In case we want to multithread later
            private int _interval;
            private List<string> _wallpaperLocations;
            private string _subreddits;
            private Curator.Utils.StretchStyles _stretchStyle;
            private int _unit;
            private Curator.UI.ConfigureForm _configureForm;

            private ConfigManager() // sets initial settings of applicatoin
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Curator\temp\settings.txt");
                this._wallpaperLocations = new List<string>();
                this._subreddits = "";
                // teting file loading.  
                if (File.Exists(path))
                {
                    using (StreamReader sr = File.OpenText(path))
                    {
                        string stat = "";
                        // set index 
                        stat = sr.ReadLine();
                        _unit = Convert.ToInt32(stat);
                        int scaleFactor = 1;
                        switch (_unit)
                        {
                            case 0:
                                scaleFactor = 1000;
                                break;
                            case 1:
                                scaleFactor = 60 * 1000;
                                break;
                            case 2:
                                scaleFactor = 60 * 60 * 1000;
                                break;
                            case 3:
                                scaleFactor = 24 * 60 * 60 * 1000;
                                break;
                            default:
                                scaleFactor = 1000;
                                break;
                        }



                        // set intervals
                        stat = sr.ReadLine();
                        _interval = Convert.ToInt32(stat);
                        _interval *= scaleFactor;
                        //for style types
                        stat = sr.ReadLine();
                        int same = String.Compare(stat, "fill");
                        bool found = false;
                        if (same == 0) { _stretchStyle = Curator.Utils.StretchStyles.Fill; found = true; }
                        same = String.Compare(stat, "fit");
                        if (same == 0) { _stretchStyle = Curator.Utils.StretchStyles.Fit; found = true; }
                        same = String.Compare(stat, "stretch");
                        if (same == 0) { _stretchStyle = Curator.Utils.StretchStyles.Stretch; found = true; }
                        same = String.Compare(stat, "center");
                        if (same == 0) { _stretchStyle = Curator.Utils.StretchStyles.Center; found = true; }
                        same = String.Compare(stat, "centerfit");
                        if (same == 0) { _stretchStyle = Curator.Utils.StretchStyles.CenterFit; found = true; }
                        same = String.Compare(stat, "tile");
                        if (same == 0) { _stretchStyle = Curator.Utils.StretchStyles.Tile; found = true; }
                        if (found == false) { _stretchStyle = Curator.Utils.StretchStyles.Stretch; }
                        // directory _wallpaperLocations list
                        if ((stat = sr.ReadLine()) != null) // check for directory informaion
                        {
                            _wallpaperLocations.Add(stat);
                        }

                        else // set default location if not found. 
                        {
                            _wallpaperLocations.Add(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures));
                        }

                        while ((stat = sr.ReadLine()) != null)
                        {
                            _wallpaperLocations.Add(stat);
                        }

                    }
                }

                else
                {
                    // Load defaults

                    this._interval = 15000;
                    this._wallpaperLocations.Add(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures));
                    this._stretchStyle = Curator.Utils.StretchStyles.Stretch;
                }

                
                

                }
                          
            
            public static ConfigManager GetInstance
            {
                get
                {
                    if (_configManagerInstance == null)
                    {
                        lock (_configManagerInstanceSync)
                        {
                            if (_configManagerInstance == null)
                            {
                                _configManagerInstance = new ConfigManager();
                            }
                        }
                    }

                    return _configManagerInstance;
                }
            }

            public Curator.UI.ConfigureForm ConfigureForm
            {
                get
                {
                    return this._configureForm;
                }
                set
                {
                    this._configureForm = value;
                }
            }

            public System.Windows.Forms.PictureBox ImagePreview
            {
                get
                {
                    return this._configureForm.ImagePreview;
                }
            }

            public void showConfigureForm()
            {
                if (this._configureForm == null || !this._configureForm.Visible)
                {
                    this._configureForm.ShowDialog();
                }
            }

            public int Interval
            {
                get
                {
                    return this._interval;
                }
                set
                {
                    this._interval = value;
                    Curator.Utils.SlideshowTimer.GetInstance.Interval = this._interval;
                }
            }

            public int Unit 
            {
                get
                {
                    return this._unit;
                }
                set 
                {
                    this._unit = value;
                }
           } 
                       

            public List<string> WallpaperLocations
            {
                get
                {
                    return this._wallpaperLocations;
                }
                set
                {
                    this._wallpaperLocations = value;
                    Curator.Utils.WallpaperChanger.GetInstance.SelectedWallpaperLocations = this._wallpaperLocations;
                }
            }

            public string subreddits
            {
                get
                {
                    return this._subreddits;
                }
                set
                {
                    this._subreddits = value;
                    
                }
            }

            public Curator.Utils.StretchStyles StretchStyle
            {
                get
                {
                    return this._stretchStyle;
                }
                set
                {
                    this._stretchStyle = value;
                    Curator.Utils.WallpaperChanger.GetInstance.StretchStyle = this._stretchStyle;
                }
            }

        }
    }