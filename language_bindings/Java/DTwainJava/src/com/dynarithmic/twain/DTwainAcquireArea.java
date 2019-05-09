package com.dynarithmic.twain;

public class DTwainAcquireArea {
	public double getLeft() {
		return left;
	}
	public void setLeft(double left) {
		this.left = left;
	}
	public double getTop() {
		return top;
	}
	public void setTop(double top) {
		this.top = top;
	}
	public double getRight() {
		return right;
	}
	public void setRight(double right) {
		this.right = right;
	}
	public double getBottom() {
		return bottom;
	}
	public void setBottom(double bottom) {
		this.bottom = bottom;
	}
	double left, top, right, bottom;
	int unitOfMeasure;
	public DTwainAcquireArea(double l, double t, double r, double b) 
		{ left = l; top = t; right = r; bottom = b; unitOfMeasure = DTwainJavaAPIConstants.DTWAIN_INCHES; }

	public DTwainAcquireArea(double l, double t, double r, double b, int unit) 
	{ left = l; top = t; right = r; bottom = b; unitOfMeasure = unit; }

	public DTwainAcquireArea() { left = top = right = bottom = -1; unitOfMeasure = DTwainJavaAPIConstants.DTWAIN_INCHES; }
	
    public DTwainAcquireArea(DTwainAcquireArea a)
    {
        left = a.left;
        top = a.top;
        right = a.right;
        bottom = a.bottom;
        unitOfMeasure = a.unitOfMeasure;
    }

	public void setAll(double l, double t, double r, double b)  { left = l; top = t; right = r; bottom = b; }
	public void setAll(double l, double t, double r, double b, int unit)	{ left = l; top = t; right = r; bottom = b; unitOfMeasure = unit;}
	
	public int getUnitOfMeasure() {
		return unitOfMeasure;
	}
	
	public void setUnitOfMeasure(int unitOfMeasure) {
		this.unitOfMeasure = unitOfMeasure;
	}
}
