/*
 * sigmoid.h
 *
 * Code generation for function 'sigmoid'
 *
 */

#ifndef __SIGMOID_H__
#define __SIGMOID_H__

/* Include files */
#include <math.h>
#include <stdlib.h>
#include <string.h>
#include "mwmathutil.h"
#include "tmwtypes.h"
#include "mex.h"
#include "emlrt.h"
#include "blas.h"
#include "rtwtypes.h"
#include "applyKerasTrainedModel_types.h"

/* Function Declarations */
extern void sigmoid(const emlrtStack *sp, const emxArray_real_T *t,
                    emxArray_real_T *s);

#endif

/* End of code generation (sigmoid.h) */