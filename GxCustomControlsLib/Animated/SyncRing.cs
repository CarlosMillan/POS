using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Gestionix.POS
{
    public class SyncRing : Control
    {
        static SyncRing()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SyncRing), new FrameworkPropertyMetadata(typeof(SyncRing)));
        }
    }
}
