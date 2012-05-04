﻿using System;
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
        protected Point controlInitialPosition;
        protected Size controlInitialSize;
        protected int minSize = 20;
        public bool drag = false;
        public int ResizeNS = 0;
        public int ResizeEW = 0;
        public int ResizableBorderWidth = 5;
        public int DefaultLength = 50;
        public Orientation Direction = Orientation.Horizontal;
        private MouseEventHandler MEvent;

        public ResizableControl()
        {
            MEvent = new MouseEventHandler(Parent_MouseUp);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            if (Direction == Orientation.Horizontal)
            {
                Direction = Orientation.Vertical;
                Height = DefaultLength;
            }
            else
            {
                Direction = Orientation.Horizontal;
                Width = DefaultLength;
            }

            setNewPosition();
            base.OnDoubleClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Point mouse = this.PointToClient(MousePosition);

            ResizeEW = 0;
            ResizeNS = 0;
            this.Cursor = Cursors.SizeAll;

            if (Direction == Orientation.Horizontal)
            {
                if (mouse.X < this.ResizableBorderWidth)
                {
                    ResizeEW = -1;
                    this.Cursor = Cursors.SizeWE;
                }
                else if (mouse.X > (this.Width - this.ResizableBorderWidth))
                {
                    ResizeEW = 1;
                    this.Cursor = Cursors.SizeWE;
                }
            }
            else
            {
                if (mouse.Y < this.ResizableBorderWidth)
                {
                    ResizeNS = -1;
                    this.Cursor = Cursors.SizeNS;
                }
                else if (mouse.Y > (this.Height - this.ResizableBorderWidth))
                {
                    ResizeNS = 1;
                    this.Cursor = Cursors.SizeNS;
                }
            }

            mouseDragInitialPosition = MousePosition;
            controlInitialPosition = this.Location;
            controlInitialSize = this.Size;
            this.drag = true;

            Parent.MouseUp += MEvent;

            base.OnMouseDown(e);
        }

        void Parent_MouseUp(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("ParenMouseUp");
            EndDrag();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            EndDrag();
            base.OnMouseUp(e);
        }

        private void EndDrag()
        {
            this.drag = false;
            ResizeNS = 0;
            ResizeEW = 0;
            Parent.MouseUp -= MEvent;
        }

        protected void setNewPosition()
        {

            this.SuspendLayout();
            //Debug.WriteLine("Resize NS " + ResizeNS);
            //Debug.WriteLine("Resize EW " + ResizeEW);

            if (ResizeNS != 0 || ResizeEW != 0)
            {
                this.Height = this.controlInitialSize.Height + (ResizeNS * (MousePosition.Y - mouseDragInitialPosition.Y));
                //Debug.WriteLine("Height resize: " + (ResizeNS * (MousePosition.Y - mouseDragInitialPosition.Y)));
                if (ResizeNS < 0)
                {
                    this.Top = this.controlInitialPosition.Y + (MousePosition.Y - mouseDragInitialPosition.Y);
                }

                this.Width = this.controlInitialSize.Width + (ResizeEW * (MousePosition.X - mouseDragInitialPosition.X));
                //Debug.WriteLine("Width resize: " + (ResizeEW * (MousePosition.X - mouseDragInitialPosition.X)));
                if (ResizeEW < 0)
                {
                    this.Left = this.controlInitialPosition.X + (MousePosition.X - mouseDragInitialPosition.X);
                }
            }
            else
            {
                this.Top = MousePosition.Y - mouseDragInitialPosition.Y + controlInitialPosition.Y;
                this.Left = MousePosition.X - mouseDragInitialPosition.X + controlInitialPosition.X;
            }


            if (this.Right > Parent.Width) this.Left = Parent.Width - this.Width;
            if (this.Bottom > Parent.Height) Top = Parent.Height - this.Height;

            this.Left = Math.Max(this.Left, 0);
            this.Top = Math.Max(this.Top, 0);
            this.Width = (Direction == Orientation.Horizontal) ? Math.Max(this.Width, minSize) : minSize;
            this.Height = (Direction == Orientation.Vertical) ? Math.Max(this.Height, minSize) : minSize;

            this.ResumeLayout();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!drag)
            {
                //if (resize) return;
                Point mouse = this.PointToClient(MousePosition);

                if (Direction == Orientation.Horizontal)
                {
                    if (mouse.X < this.ResizableBorderWidth)
                    {
                        this.Cursor = Cursors.SizeWE;
                    }
                    else if (mouse.X > (this.Width - this.ResizableBorderWidth))
                    {
                        this.Cursor = Cursors.SizeWE;
                    }
                }
                else
                {
                    if (mouse.Y < this.ResizableBorderWidth)
                    {
                        this.Cursor = Cursors.SizeNS;
                    }
                    else if (mouse.Y > (this.Height - this.ResizableBorderWidth))
                    {
                        this.Cursor = Cursors.SizeNS;
                    }
                }

                //if (mouse.X < this.ResizableBorderWidth)
                //{
                //    this.Cursor = Cursors.SizeWE;
                //    if (mouse.Y < this.ResizableBorderWidth)
                //        this.Cursor = Cursors.SizeNWSE;
                //    else if (mouse.Y > (this.Height - this.ResizableBorderWidth))
                //        this.Cursor = Cursors.SizeNESW;
                //}
                //else if (mouse.X > (this.Width - this.ResizableBorderWidth))
                //{
                //    this.Cursor = Cursors.SizeWE;
                //    if (mouse.Y < this.ResizableBorderWidth)
                //        this.Cursor = Cursors.SizeNESW;
                //    else if (mouse.Y > (this.Height - this.ResizableBorderWidth))
                //        this.Cursor = Cursors.SizeNWSE;
                //}
                //else
                //{
                //    this.Cursor = Cursors.SizeAll;
                //    if (mouse.Y < this.ResizableBorderWidth)
                //        this.Cursor = Cursors.SizeNS;
                //    else if (mouse.Y > (this.Height - this.ResizableBorderWidth))
                //        this.Cursor = Cursors.SizeNS;
                //}
            }
            else
            {
                setNewPosition();
            }
            base.OnMouseMove(e);
        }
    }
}