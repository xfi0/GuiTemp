using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiTemp.Menu
{
    public class ButtonInfo
    {   public string buttonText = "PlaceHolder";
        public string overlapText = null;
        public Action method = null;
        public Action enableMethod = null;
        public Action disableMethod = null;
        public bool enabled = false;
        public bool isTogglable = true;
        public string toolTip = "I Didnt Add A ToolTip Because Its Probably Common Sense, Or I Forgot";
    }
}
