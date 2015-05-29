var registerModel = function () {
    var self = this;
    self.Email = ko.observable("");
    self.Password = ko.observable("");
};
var userModel = function () {
    var self = this;
    self.Id = ko.observable("");
    self.Email = ko.observable("");
};

var viewModel = function () {
    var self = this;

    self.List = ko.observableArray([]);

    self.RegisterModel = new registerModel();
    self.UserModel = new userModel();

    self.Register = function () {
        var param = ko.toJS(self.RegisterModel);
        $.ajax({
            url: "Users/Register",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: ko.utils.stringifyJson(param),
            success: function (result) {
                console.log(result);
                if (result.success) {
                    toastr.success(result.message);
                    self.InitUsers();
                } else {
                    toastr.error(result.message);
                }
            },
            error: function (xhr, err) {
                console.log("readyState: " + xhr.readyState + "\nstatus: " + xhr.status + "\nresponseText: " + xhr.responseText);
            }
        });
    };
    self.Delete = function (user) {
        if (confirm("Are you sure you want to delete this data?")) {
            self.UserModel.Id(user.Id);
            self.UserModel.Email(user.Email);

            var param = ko.toJS(self.UserModel);

            $.ajax({
                url: "Users/Delete",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: ko.utils.stringifyJson(param),
                success: function (result) {
                    console.log(result);
                    if (result.success) {
                        toastr.success(result.message);
                        self.InitUsers();
                    } else {
                        toastr.error(result.message);
                    }
                },
                error: function (xhr, err) {
                    console.log("readyState: " + xhr.readyState + "\nstatus: " + xhr.status + "\nresponseText: " + xhr.responseText);
                }
            });
        }
        
    };

    self.InitUsers = function () {
        $.ajax({
            url: "Users/GetUsers",
            type: "GET",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                self.List(result);
            },
            error: function (xhr, err) {
                console.log("readyState: " + xhr.readyState + "\nstatus: " + xhr.status + "\nresponseText: " + xhr.responseText);
            }
        });
    };

    self.InitToastr = function () {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "positionClass": "toast-top-right",
            "showDuration": "300",
            "hideDuration": "5000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
    };
};
$(document).ready(function () {
    var model = new viewModel();
    model.InitUsers();
    model.InitToastr();

    ko.applyBindings(model);
});