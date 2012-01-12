using System;


namespace Radiology.Utility
{
    public class Utility
    {
        public static void OnFormSubmitCompleted(SubmitOperation so)
        {
            if (so.HasError)
            {
                MessageBox.Show(string.Format("Submit Failed: {0}", so.Error.Message));
                so.MarkErrorAsHandled();
            }
            else
            {
                MessageBox.Show("Saved Successfully");
            }
        }
    }
}
