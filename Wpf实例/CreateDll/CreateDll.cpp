// CreateDll.cpp : ���� DLL Ӧ�ó���ĵ���������
//

#include "stdafx.h"
#include "CreateDll.h"
#include "dinput.h"

PBYTE __stdcall test01(PBYTE a,int b, int c)
{
	return  a + b + c;
}
int __stdcall test02(int a ,int b) {
	return a - b;
}
