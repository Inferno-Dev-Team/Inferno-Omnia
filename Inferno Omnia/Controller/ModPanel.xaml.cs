using InfernoOmnia.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InfernoOmnia.Controller {
    public partial class ModPanel : UserControl
    {
        public Mod mod { get; set; }

        public ModPanel()
        {
            InitializeComponent();

            if (!mod.Equals(null))
                DataContext = mod;
        }
    }
}
