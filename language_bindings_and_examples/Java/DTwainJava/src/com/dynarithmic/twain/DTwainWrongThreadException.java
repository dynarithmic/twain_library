package com.dynarithmic.twain;
public class DTwainWrongThreadException extends Exception
{
     String message;
     
     public DTwainWrongThreadException(String reason)
     {
         message = reason;
     }
     
     public String getMessage()
     {
         return message;
     }
}