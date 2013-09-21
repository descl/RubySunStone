using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Resources;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Media;
//using Windows.Phone.Media.Capture;

namespace RubySunStoneMobile.Utils
{
	public class CaptureScreen
	{
        static public Boolean capture(int quality, bool landscape, double latitude, double longitude)
		{
			try
			{
				PhoneApplicationFrame frame = (PhoneApplicationFrame)Application.Current.RootVisual;
				WriteableBitmap bitmap;
                if (landscape)
                {
                    bitmap = new WriteableBitmap((int)frame.ActualHeight, (int)frame.ActualWidth);
                }
                else
                {
                    bitmap = new WriteableBitmap((int)frame.ActualWidth, (int)frame.ActualHeight);
                }
               
				bitmap.Render(frame, null);
				bitmap.Invalidate();

				string fileName = latitude.ToString() + "-" + longitude.ToString() + DateTime.Now.ToString("'-'yyyyMMddHHmmssfff'.jpg'");
				IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
				if (storage.FileExists(fileName))
					return false;

				IsolatedStorageFileStream stream = storage.CreateFile(fileName);
				bitmap.SaveJpeg(stream, bitmap.PixelWidth, bitmap.PixelHeight, 0, quality);
				stream.Close();

				stream = storage.OpenFile(fileName, FileMode.Open, FileAccess.Read);
				MediaLibrary mediaLibrary = new MediaLibrary();
				Picture picture = mediaLibrary.SavePicture(fileName, stream);
				stream.Close();

				storage.DeleteFile(fileName);
                

			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
				return false;
			}

			return true;
		}
	}
}
