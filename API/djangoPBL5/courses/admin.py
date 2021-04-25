from django.contrib import admin

# Register your models here.
from courses.models import Account


class AccountAdmin(admin.ModelAdmin):
   list_display = ('id','password', 'level', 'group')


admin.site.register(Account, AccountAdmin)
