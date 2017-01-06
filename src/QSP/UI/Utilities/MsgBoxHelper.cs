﻿using System.Drawing;
using System.Windows.Forms;
using QSP.UI.Forms;

namespace QSP.UI.Utilities
{
    public static class MsgBoxHelper
    {
        public static DialogResult ShowError(string text, string caption = "")
        {
            return MessageBox.Show(
                text,
                caption,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static DialogResult ShowWarning(string text, string caption = "")
        {
            return MessageBox.Show(
                text,
                caption,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        public static DialogResult ShowInfo(string text, string caption = "")
        {
            return MessageBox.Show(
                text,
                caption,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static int ShowDialog(string text, Icon icon, string caption, 
            string[] buttonTxt, int defaultBtn)
        {
            var frm = new MsgBoxForm();
            frm.Init(text, icon, caption, buttonTxt, defaultBtn);
            frm.ShowDialog();
            return 0;
        }
    }
}
