package com.dynarithmic.twain;
import java.lang.Math;
public class DTwainRangeInt
{
    private int minVal;
    private int maxVal;
    private int step;
    private int defVal;
    private int curVal;
    public static final int RANGE_MIN_OFFSET = 0;
    public static final int RANGE_MAX_OFFSET = 1;
    public static final int RANGE_STEP_OFFSET = 2;
    public static final int RANGE_DEFVAL_OFFSET = 3;
    public static final int RANGE_CURVAL_OFFSET = 4;
    
    
    public DTwainRangeInt()
    {
        minVal = 0;
        maxVal = 0;
        step = 0;
        defVal = 0;
        curVal = 0;
    }

    public DTwainRangeInt(final int minv, final int maxv, final int st, final int def, final int cur)
    {
        minVal = minv;
        maxVal = maxv;
        step = st;
        defVal = def;
        curVal = cur;
    }

    public DTwainRangeInt(final int [] v)
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

    public int getMin() { return minVal; }
    public int getMax() { return maxVal; }
    public int getStep() { return step; }
    public int getDefault() { return defVal; }
    public int getCurrent() { return curVal; }

    public void setMin(final int minv) { minVal = minv; }
    public void setMax(final int maxv) { maxVal = maxv; }
    public void setStep(final int st)  { step = st; }
    public void setCurrent(final int cur) { curVal = cur; }

    public int [] expand()
    {
    	final int lCount = count();
    	final int [] temp = new int [lCount];
    	for (int i = 0; i < lCount; ++i )    	
    		temp[i] = minVal + (step * i);
    	return temp;
    }

    public int count() 
    {
    	return Math.abs(maxVal - minVal) / step + 1;
    }

    public int getNearestValue(final int value, int roundtype) 
    {
    	int lInVal = value;
    	if ( step == 0 )
    		return minVal;

    	if ( lInVal < minVal )
    		return minVal;
    	
    	if ( lInVal > maxVal )
    		return maxVal;
    	
    	int remainder; 
    	int dividend;
    	
    	int bias = 0;
    	if ( minVal != 0 )
    		bias = -minVal;
    	lInVal += bias;
    	remainder = Math.abs(lInVal % step);
    	dividend = lInVal / step;
    	
    	if ( remainder == 0 )
    		return lInVal - bias;
    	
    	if ( roundtype == DTwainJavaAPIConstants.DTWAIN_ROUNDNEAREST) 
    	{
    		if ( remainder >= Math.abs(step) / 2 )
    			roundtype = DTwainJavaAPIConstants.DTWAIN_ROUNDUP;
    		else
    			roundtype = DTwainJavaAPIConstants.DTWAIN_ROUNDDOWN;
    	}
    	
    	if ( roundtype == DTwainJavaAPIConstants.DTWAIN_ROUNDDOWN)
    		return (dividend * step) - bias;
   		return ((dividend + 1) * step) - bias;
    }
    
    public int getExpandedValue(final int nPos) throws DTwainRuntimeException
    {
    	if ( nPos < 0 )
    		throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_INVALID_PARAM);
    	return minVal + ( step * nPos );
    }
    
    public int getPosition(final int value) throws DTwainRuntimeException 
    {
    	if ( step == 0 )
    		throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_INVALID_RANGE);
    	return (value - minVal) / step;
    }
}
