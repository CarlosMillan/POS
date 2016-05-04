using System.Windows;
using System.Windows.Controls;

namespace Gestionix.POS.GUI
{
    public class SyncRing : Control
    {
        static SyncRing()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SyncRing), new FrameworkPropertyMetadata(typeof(SyncRing)));
        }
    }
}
