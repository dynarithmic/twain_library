package com.dynarithmic.twain;
import java.lang.Math;
public class DTwainRangeDouble
{
    private double minVal;
    private double maxVal;
    private double step;
    private double defVal;
    private double curVal;
    public static final int RANGE_MIN_OFFSET = 0;
    public static final int RANGE_MAX_OFFSET = 1;
    public static final int RANGE_STEP_OFFSET = 2;
    public static final int RANGE_DEFVAL_OFFSET = 3;
    public static final int RANGE_CURVAL_OFFSET = 4;

    public DTwainRangeDouble()
    {
        minVal = 0;
        maxVal = 0;
        step = 0;
        defVal = 0;
        curVal = 0;
    }

    public DTwainRangeDouble(double minv, double maxv, double st, double def, double cur)
    {
        minVal = minv;
        maxVal = maxv;
        step = st;
        defVal = def;
        curVal = cur;
    }

    public DTwainRangeDouble(double [] v)
    {
       if (v.length >= 5)
       {
         minVal = v[RANGE_MIN_OFFSET];
         maxVal = v[RANGE_MAX_OFFSET];
         step   = v[RANGE_STEP_OFFSET];
         defVal = v[RANGE_DEFVAL_OFFSET];
         curVal = v[RANGE_CURVAL_OFFSET];
       }
       else
       {
          minVal = 0;
          maxVal = 0;
          step = 0;
          defVal = 0;
          curVal = 0;
        }
    }

    public double getMin() { return minVal; }
    public double getMax() { return maxVal; }
    public double getStep() { return step; }
    public double getDefault() { return defVal; }
    public double getCurrent() { return curVal; }

    public void setMin(double minv) { minVal = minv; }
    public void setMax(double maxv) { maxVal = maxv; }
    public void setStep(double st)  { step = st; }
    public void setCurrent(double cur) { curVal = cur; }

    private static boolean FLOAT_CLOSE(double x, double y) {
    	return (Math.abs(x - y) <= DTwainJavaAPIConstants.DTWAIN_FLOATDELTA);
    }
    
    public double [] expand()
    {
      	final int lCount = count();
    	final double [] temp = new double [lCount];
    	for (int i = 0; i < lCount; ++i )    	
    		temp[i] = minVal + (step * i);
    	return temp;
    }

    public int count()
    {
    	return (int)(Math.abs(maxVal - minVal) / step) + 1;
    }

    public double getNearestValue(double value, int roundtype) 
    {
    	if (FLOAT_CLOSE(minVal, maxVal) ||
    		FLOAT_CLOSE(0.0, step) ||
    		value < minVal )
    		return minVal;
    	else
    	if ( FLOAT_CLOSE(maxVal, value) || (value > maxVal) )
    		return maxVal;
    	
    	double bias = 0.0;
    	if ( minVal != 0.0 )
    		bias = -minVal;
    	value += bias;
    	double remainder = Math.abs( value % step);
    	double dividend = value / step;
    	if ( FLOAT_CLOSE(remainder, 0.0 )) 	{
    		return value - bias;
    	}
    	
    	//check rounding
    	if ( roundtype == DTwainJavaAPIConstants.DTWAIN_ROUNDNEAREST ){
    		if ( remainder >= Math.abs(step) / 2.0)
    			roundtype = DTwainJavaAPIConstants.DTWAIN_ROUNDUP;
    		else
    			roundtype = DTwainJavaAPIConstants.DTWAIN_ROUNDDOWN;
    	}
    	if ( roundtype == DTwainJavaAPIConstants.DTWAIN_ROUNDDOWN )
    		return dividend * step  - bias;
    	else
   		if ( roundtype == DTwainJavaAPIConstants.DTWAIN_ROUNDUP)
   			return (dividend + 1.0) * step - bias;
    	return 0.0;
    }

    public double getExpandedValue(int nPos) throws DTwainRuntimeException
    {
    	if ( nPos < 0 )
    		throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_INVALID_PARAM);
    	return minVal + ( step * nPos );
    }
    
    public double getPosition(double value) throws DTwainRuntimeException 
    {
    	if ( step == 0 )
    		throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_INVALID_RANGE);
    	return (value - minVal) / step;
    }

}
