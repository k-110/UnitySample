set MY_VENV="..\venv\Scripts\activate.bat"


if exist %MY_VENV% goto MY_PACK_INSTALL
rem ----------------------------
rem - 仮想環境の作成
rem ----------------------------
python -m venv ..\venv


:MY_PACK_INSTALL
rem ----------------------------
rem - パッケージのインストール
rem ----------------------------
call %MY_VENV%
pip install django

pause
