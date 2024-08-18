package com.dynarithmic.twain;
    public class DTwainFrame extends DTwainAcquireArea {
        public DTwainFrame(double l, double t, double r, double b) 
        { super(l,t,r,b);}

        public DTwainFrame() 
        { super(); }

        public DTwainFrame(DTwainFrame f)
        {super(f); }
    };