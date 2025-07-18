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
        public Action method = null; // is ran everyframe
        public Action enableMethod = null; // ran on button/gui option enable but only once
        public Action disableMethod = null; // ran on button/gui option disable but only once
        public bool enabled = false; // if the button/gui option comes enabled by default
        public bool isTogglable = true; // if the button/gui option is toggleable
        public string toolTip = "The Menu Owner Forgot A Tool Tip Or Notifs Broke!!!!";
    }
}
