var InputSourceModel = function () {
    var self = this;
    self.mode = ko.observable();

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

    self.inputSources = ko.observableArray();
    self.selectedInputSource = ko.observable();
    self.getInputSource = function () {
        $.ajax({
            url: "/api/inputsource/all",
            method: "get"
        }).done(function (items) {  
            self.inputSources.removeAll();
            $.each(items, function (index, item) {
                self.inputSources.push(self.convertToKoObject(item))
            });
            console.log(self.inputSources());
        })
    }
    self.showInfo = function () {
        $.ajax({
            url: '/api/inputsource/info',
            type: 'GET'
        }).done(function (data) {
            self.selectedInputSource(data);
            console.log(self.selectedInputSource())
        });
    }

    self.showModel = function () {
        self.mode('create');
        self.showInfo();
        $('#ghiLai').modal('show');
    }
   
    self.create = function (item) {      
        $.ajax({
            url: '/api/inputsource/add',
            type: 'POST',
            data: ko.mapping.toJSON(item),
            contentType: 'application/json',
            dataType: 'json',
            success: function (data) {
                self.getInputSource();
                $('#ghiLai').modal('hide');
                toastr.success("Đã thêm mới dữ liệu", "Thành công!");
            },
            error: function () {
                toastr.error("Đã có lỗi", "Thất bại!");
            }
        });
    }
    self.remove = function (item) {
        var id = item.ID();
        $.ajax({
            url: '/api/inputsource/' + id,
            type: 'DELETE',
            contentType: 'application/json',
            dataType: 'json',
            success: function (data) {
                self.getInputSource();
                toastr.success("Đã xóa dữ liệu", "Thành công!");
            },
            error: function () {
                toastr.error("Đã có lỗi", "Thất bại!");
            }
        });
    }
    self.confirmRemove = function (item) {
        var result = confirm("Bạn muốn xóa chứ ?");
        if (result) {
            self.remove(item);
        }
    }
    self.edit = function (item) {
        self.mode('update');
        self.selectedInputSource(item);
        $('#ghiLai').modal('show');
    }
    self.update = function (item) {
        $.ajax({
            url: '/api/inputsource/' + item.ID(),
            type: 'PUT',
            data: ko.mapping.toJSON(item),
            contentType: 'application/json',
            dataType: 'json',
            success: function (data) {
                self.getInputSource();
                $('#ghiLai').modal('hide');
                toastr.success("Đã sửa dữ liệu", "Thành công!");
            },
            error: function () {
                toastr.error("Đã có lỗi", "Thất bại!");
            }
        });
    }


}
$(function () {
    var inputSourceModel = new InputSourceModel();
    inputSourceModel.getInputSource();
    ko.applyBindings(inputSourceModel);
});