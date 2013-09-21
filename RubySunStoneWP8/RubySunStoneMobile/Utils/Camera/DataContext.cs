﻿/*
 * Copyright © 2012 Nokia Corporation. All rights reserved.
 * Nokia and Nokia Connecting People are registered trademarks of Nokia Corporation. 
 * Other product and company names mentioned herein may be trademarks
 * or trade names of their respective owners. 
 * See LICENSE.TXT for license information.
 */

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Windows.Phone.Media.Capture;

namespace RubySunStoneMobile.Utils.Camera
{
    /// <summary>
    /// CameraExplorer.DataContext holds all application widely used instances, like parameters,
    /// camera instance and image memory stream.
    /// </summary>
    class DataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static DataContext _singleton;
        private PhotoCaptureDevice _device = null;
        private ObservableCollection<Parameter> _parameters = new ObservableCollection<Parameter>();

        /// <summary>
        /// Singleton instance accessor.
        /// </summary>
        public static DataContext Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = new DataContext();
                }

                return _singleton;
            }
        }

        /// <summary>
        /// Collection of camera parameters.
        /// </summary>
        public ObservableCollection<Parameter> Parameters
        {
            get
            {
                return _parameters;
            }

            private set
            {
                if (_parameters != value)
                {
                    _parameters = value;


                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Parameters"));
                    }
                }
            }
        }

        /// <summary>
        /// Camera instance. Setting new camera instance to this property causes the Parameters
        /// property to be updated as well with the new parameters from the new camera.
        /// </summary>
        public PhotoCaptureDevice Device
        {
            get
            {
                return _device;
            }

            set
            {
                if (_device != value)
                {
                    _device = value;
                    
                    if (_device != null)
                    {
                        ObservableCollection<Parameter> newParameters = new ObservableCollection<Parameter>();

                        Action<Parameter> addParameter = (Parameter parameter) =>
                        {
                            if (parameter.Supported && parameter.Modifiable)
                            {
                                try
                                {
                                    parameter.Refresh();
                                    parameter.SetDefault();

                                    newParameters.Add(parameter);
                                }
                                catch (Exception)
                                {
                                    System.Diagnostics.Debug.WriteLine("Setting default to " + parameter.Name.ToLower() + " failed");
                                }
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("Parameter " + parameter.Name.ToLower() + " is not supported or not modifiable");
                            }
                        };

                        addParameter(new SceneModeParameter(_device));
                        addParameter(new WhiteBalancePresetParameter(_device));
                        addParameter(new FlashModeParameter(_device));
                        //addParameter(new FlashPowerParameter(_device));
                        addParameter(new IsoParameter(_device));
                        //addParameter(new ExposureCompensationParameter(_device));
                        addParameter(new ExposureTimeParameter(_device));
                        addParameter(new AutoFocusRangeParameter(_device));
                        addParameter(new FocusIlluminationModeParameter(_device));
                        addParameter(new CaptureResolutionParameter(_device));

                        Parameters = newParameters;
                    }

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Device"));
                    }
                }
            }
        }

        /// <summary>
        /// Memory stream to hold the image data captured in MainPage but consumed in PreviewPage.
        /// </summary>
        public MemoryStream ImageStream { get; set; }
    }
}