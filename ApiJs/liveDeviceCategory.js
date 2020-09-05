var DeviceCategoryModel = function () {
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
  
    self.selectedCategory = ko.observable();
    self.categories = ko.observableArray();
    
    self.getAll = function () {
        $.ajax({
            url: '/api/devicecatagory/all',
            type: 'GET'         
        }).done(function (data) {
            self.categories.removeAll();
            $.each(data, function (index, item) {
                self.categories.push(self.convertToKoObject(item))
            })
            
        });
    }
    self.showInfo = function () {
        $.ajax({
            url: '/api/devicecatagory/info',
            type: 'GET'
        }).done(function (data) {         
            self.selectedCategory(data);
        });
    }
   
    self.showModel = function () {   
        self.mode('create');
        self.showInfo();    
        $('#ghiLai').modal('show');
      
       
    }

    self.create = function (item) {
        $.ajax({
            url: '/api/devicecatagory/add',
            type: 'POST',
            data: ko.mapping.toJSON(item),
            contentType: 'application/json',
            dataType: 'json'
        }).success(function (data) {        
            self.getAll();          
            $('#ghiLai').modal('hide');
        });
    }

    self.remove = function (item) {    
        var id = item.ID();
        $.ajax({
            url: '/api/devicecatagory/' + id,
            type: 'DELETE',
            contentType: 'application/json',
            dataType: 'json'
        }).success(function (data) {
            self.getAll();
        });    
    }
    self.confirmRemove = function (item) {
        var result = confirm("Bạn muốn xóa chứ ??");
        if (result) {
            self.remove(item);
        }
    }

    self.edit = function (item) {
        self.mode('update');
        self.selectedCategory(item);
        $('#ghiLai').modal('show');
    }

    self.update = function (item) {     
        $.ajax({
            url: '/api/devicecatagory/' + item.ID(),
            type: 'PUT',
            data: ko.mapping.toJSON(item),
            contentType: 'application/json',
            dataType: 'json'
        }).success(function (data) {
            self.getAll();          
            $('#ghiLai').modal('hide');
           
        }); 
    }
}

$(function () {
    var deviceCategoryModel = new DeviceCategoryModel();
    deviceCategoryModel.getAll();
    ko.applyBindings(deviceCategoryModel);
});