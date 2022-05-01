from django.urls import path
from memo import views

app_name = 'memo'
urlpatterns = [
    path('', views.memoHomeView.as_view()),
]
