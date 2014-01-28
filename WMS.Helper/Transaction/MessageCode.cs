using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.Helper.Transaction
{
    public class MessageCode
    {
        #region Message Code
        public static string saved = "Successfully saved!";
        public static string modified = "Successfully modified!";
        public static string deleted = "Successfully deleted!";
        public static string error = "An error has occured";
        public static string existed = "Item already exists";
        public static string valid = "Item is valid.";
        public static string empty = "Item is empty.";
        #endregion

        #region Message
        public static string GetMessage(string code)
        {
            if (code.Equals(StatusCode.saved))
            {
                return MessageCode.saved;
            }
            else if (code.Equals(StatusCode.modified))
            {
                return MessageCode.modified;
            }
            return string.Empty;
        }
        #endregion
    }
}
