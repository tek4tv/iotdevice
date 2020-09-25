var LoginModel = function () {
    var self = this;
    self.convertToKoObject = function (data) {
        var newObj = ko.mapping.fromJS(data);
        return newObj;
    }
    self.convertToJson = function (item) {
        if (item == null || item == "") {
            return [];
        } else {
            return JSON.parse(item);
        }
    };
   
    self.UserName = ko.observable();
    self.PassWord = ko.observable();
    self.submit = function () {
        var viewModel = {
            UserName: self.UserName,
            PassWord: self.PassWord
        }              
        $.ajax({
            url: '/Login/LoginAsync' ,
            type: 'POST',
            data: ko.mapping.toJSON(viewModel),
            contentType: 'application/json',
            dataType: 'json',
            success: function (response) {
                if (response.Success) {
                    window.location.href = '/Home/Index'
                }
                else {
                    toastr.error("Sai tài khoản hoặc mật khẩu", "Vui lòng nhập lại!");
                }              
            },
            error: function () {
                toastr.error("Sai tài khoản hoặc mật khẩu", "Vui lòng nhập lại!");
            }
        })
    }
    self.enterSubmit = function (d, e) {
        e.keyCode === 13 && self.submit();
        return true;
    };
  
}
$(function () {
    var loginModel = new LoginModel();   
    ko.applyBindings(loginModel);
});