using DYMO.Label.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace modern_label
{
    class Dymo_provider
    {
        
       
        public Dymo_provider()
        {
          
        }


        public BitmapImage generate_preview (ILabel label)
        {

            string path = Directory.GetCurrentDirectory();
            ILabelRenderParams renderParams = new LabelRenderParams();

            renderParams.LabelColor = DYMO.Label.Framework.Colors.White;
            renderParams.ShadowColor = DYMO.Label.Framework.Colors.DarkGray;
            renderParams.ShadowDepth = 3;
            renderParams.PngUseDisplayResolution = true;
            byte[] pngData = label.RenderAsPng(null, renderParams);
            System.Drawing.Image.FromStream(new MemoryStream(pngData)).Save(path + @"\\preview.png");
            System.Windows.Media.Imaging.BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(pngData);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            // bitmap.UriSource = new Uri(path + @"\\preview.png");
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }

        //read and create label by usng source.label located in Debug folder
        //returns the preview image
        public ILabel generate_label (RefrubHistoryObj spec)
        {
            string path = Directory.GetCurrentDirectory();
            var label = DYMO.Label.Framework.Label.Open(path + @"\\source.label");
            label.SetObjectText("cpu", spec.cpu);
            label.SetObjectText("asset_tag", spec.asset_tag.ToString());
            label.SetObjectText("manu", spec.model);
            label.SetObjectText("ram", spec.ram + "GB");
            label.SetObjectText("hdd", spec.hdd + "GB");
            label.SetObjectText("time", DateTime.Now.ToString());
            label.SetObjectText("refur", spec.refurbisher);
            label.SetObjectText("BARCODE", spec.sku);
            label.SetObjectText("channel", spec.channel);
            label.SetObjectText("pallet", spec.sku);
            label.SetObjectText("serial", spec.serial);


            //create preview for the label 
            return label;
        }

    }
}
