using System;
using Xamarin.Forms;
using System.Reflection;
using Xamarin.Forms.Xaml;

namespace SmartUro.Extensions
{
    /**
     * This extension enables the lookup of images relative to the project.
     * In XAML markup it is then possible like so:
     *             &lt;Image 
        Grid.Row="5"
        Grid.Column="1"
        Source="{imageExtension:ImageResource BDSM.Application.Resources.bluetooth_icon.png }"
        /&gt;
        
        The source property is the important part.
     */
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if(Source == null)
            {
                System.Diagnostics.Debug.WriteLine("Source is Null");
                return null;
            }

            System.Diagnostics.Debug.WriteLine("Source is NOT Null AND THIS MESSAGE SUCKS");
            System.Diagnostics.Debug.WriteLine($"{Source}");
            return ImageSource.FromResource(Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);
        }
    }
}