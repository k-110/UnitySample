from django.shortcuts import render
from django.views.generic import TemplateView
from django.views.decorators.clickjacking import xframe_options_exempt
from memo.my_memo import read_memo, write_memo

# home
class memoHomeView(TemplateView):
    template_name = 'memo/home.html'

    # get_context_data
    def get_context_data(self):
        memo_data = read_memo()
        context = {
            'memo_data': memo_data,
        }
        return context

    # get
    def get(self, request, *args, **kwargs):
        context = self.get_context_data()
        return render(request, self.template_name, context)
    # post
    def post(self, request, *args, **kwargs):
        memo_data = request.POST['memo']
        write_memo(memo_data)
        return self.get(request, *args, **kwargs)
