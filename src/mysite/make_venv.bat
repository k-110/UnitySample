set MY_VENV="..\venv\Scripts\activate.bat"


if exist %MY_VENV% goto MY_PACK_INSTALL
rem ----------------------------
rem - ���z���̍쐬
rem ----------------------------
python -m venv ..\venv


:MY_PACK_INSTALL
rem ----------------------------
rem - �p�b�P�[�W�̃C���X�g�[��
rem ----------------------------
call %MY_VENV%
pip install django

pause
