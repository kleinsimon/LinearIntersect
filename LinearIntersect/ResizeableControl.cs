using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Data;
using System.Linq;
using System.IO;

namespace LinearIntersect
{
    public class ResizableControl : Control
    {
        protected Point mouseDragInitialPosition;
        public bool drag = false;
        public int ResizeNS = 0;
        public int ResizeEW = 0;
        public int ResizableBorderWidth = 5;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            mouseDragInitialPosition = MousePosition;
            this.drag = true;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.drag = false;
            base.OnMouseUp(e);
        }

        protected void setNewPosition()
        {
            if (ResizeNS != 0 || ResizeEW != 0)
            {
                this.Height += (ResizeNS * (MousePosition.Y - mouseDragInitialPosition.Y));
                if (ResizeNS < 0)
                {
                    this.Top += (MousePosition.Y - mouseDragInitialPosition.Y);
                }

                this.Width += (ResizeEW * (MousePosition.X - mouseDragInitialPosition.X));
                if (ResizeEW < 0)
                {
                    this.Left += (MousePosition.X - mouseDragInitialPosition.X);
                }
                ResizeNS = 0;
                ResizeEW = 0;
            }
            else
            {
                this.Top += MousePosition.Y - mouseDragInitialPosition.Y;
                this.Left += MousePosition.X - mouseDragInitialPosition.X;
            }
            if (this.Left < 0) this.Left = 0;
            if (this.Right > Parent.Width) this.Left = Parent.Width - this.Width;
            if (this.Top < 0) this.Top = 0;
            if (this.Bottom > Parent.Height) Top = Parent.Height - this.Height;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!drag)
            {
                Point mouse = this.PointToClient(MousePosition);
                if (mouse.X < this.ResizableBorderWidth)
                {
                    ResizeEW = -1;
                    this.Cursor = Cursors.SizeWE;
                    if (mouse.Y < this.ResizableBorderWidth)
                    {
                        ResizeNS = -1;
                        this.Cursor = Cursors.SizeNWSE;
                    }
                    else
                    {
                        if (mouse.Y > (this.Height - this.ResizableBorderWidth))
                        {
                            ResizeNS = 1;
                            this.Cursor = Cursors.SizeNESW;
                        }
                    }
                }
                else
                {
                    if (mouse.X > (this.Width - this.ResizableBorderWidth))
                    {
                        ResizeEW = 1;
                        this.Cursor = Cursors.SizeWE;
                        if (mouse.Y < this.ResizableBorderWidth)
                        {
                            ResizeNS = -1;
                            this.Cursor = Cursors.SizeNESW;
                        }
                        else
                        {
                            if (mouse.Y > (this.Height - this.ResizableBorderWidth))
                            {
                                ResizeNS = 1;
                                this.Cursor = Cursors.SizeNWSE;
                            }
                        }
                    }
                    else
                    {
                        ResizeEW = 0;
                        ResizeNS = 0;
                        this.Cursor = Cursors.SizeAll;
                        if (mouse.Y < this.ResizableBorderWidth)
                        {
                            ResizeNS = -1;
                            this.Cursor = Cursors.SizeNS;
                        }
                        else
                        {
                            if (mouse.Y > (this.Height - this.ResizableBorderWidth))
                            {
                                ResizeNS = 1;
                                this.Cursor = Cursors.SizeNS;
                            }
                        }
                    }
                }
             
            }
            else
            {
                setNewPosition();
            }
            base.OnMouseMove(e);
        }
    }
}