#include <windows.h>
#include <string.h>
#include <stdio.h>
#include <math.h>
#include <io.h>
#include <direct.h>
//#include <memory.h>
#include <assert.h>
#include <stdlib.h>
#include "commdlg.h"
#include "dib.h"
#include "dlgs.h"
#include "errors.h"
#include "paint.h"
#include "palette.h"
#define RECTWIDTH(lpRect)     ((lpRect)->right - (lpRect)->left)
#define RECTHEIGHT(lpRect)    ((lpRect)->bottom - (lpRect)->top)

