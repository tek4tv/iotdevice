var DeviceModel = function () {
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
    
    self.categories = ko.observableArray();
   
    self.getCategories = function () {
        $.ajax({
            url: '/api/devicecatagory/all',
            type: 'GET'
        }).done(function (items) {           
            $.each(items, function (index, item) {
                self.categories.push(self.convertToKoObject(item))
            });
            self.createTreeCategory();         
        });
    }
  
    self.selectedGroup = ko.observable();
   
    self.createTreeCategory = function () {
        var data = [];
        $.each(self.categories(), function (idx, item) {
            data.push({ id: item.ID(), text: item.Name(), parentID: item.ParentID()});
        });
     
        $("#tree_group2").jstree({
            "core": {
                "themes": {
                    "responsive": false
                },
                // so that create works
                "check_callback": true,
                'data': getGroupModel(data)
            },
            "types": {
                "default": {
                    "icon": "fa fa-folder icon-state-warning icon-lg"
                },
                "file": {
                    "icon": "fa fa-file icon-state-warning icon-lg"
                }
            },
            "state": { "key": "demo2" },
            "plugins": ["contextmenu", "dnd", "state", "types"]
        });
        $("#tree_group2").on("select_node.jstree",
            function (evt, data) {
                var nodeId = $('#tree_group2').jstree().get_selected("id")[0].id;
                var nodeName = $('#tree_group2').jstree().get_selected("id")[0].text;
                var gr = { ID: nodeId, NAME: nodeName };
                self.selectedGroup(self.convertToKoObject(gr));
                self.loadDeviceByGroup(self.selectedGroup());
               
            }
        );
    }
   
    self.deviceByCategory = ko.observableArray();
    self.selectedDevice = ko.observable();
    self.loadDeviceByGroup = function (item) {
        $.ajax({
            url: '/api/device/category/' + item.ID(),
            type: 'GET',
            contentType: 'application/json',
            dataType: 'json'
        }).success(function (data) {
            self.deviceByCategory.removeAll();
            $.each(data, function (index, item) {
                self.deviceByCategory.push(self.convertToKoObject(item))
            });
          
        })
    }
    self.showInfo = function () {
        $.ajax({
            url: '/api/device/info',
            type: 'GET'
        }).done(function (data) {
            self.selectedDevice(data)
        });
    }

    self.showModel = function () {
       self.mode('create');
         self.showInfo();
        $('#ghiLai').modal('show');
    }

    self.create = function (item) {      
        item.LiveCategoryID = self.selectedGroup().ID()
        console.log(item)
        $.ajax({
            url: '/api/device/add',
            type: 'POST',
            data: ko.mapping.toJSON(item),
            contentType: 'application/json',
            dataType: 'json',
            success: function (data) {
                $('#ghiLai').modal('hide');
                self.loadDeviceByGroup(self.selectedGroup());
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
            url: '/api/Device/' + id,
            type: 'DELETE',
            contentType: 'application/json',
            dataType: 'json',
            success: function (data) {
                self.loadDeviceByGroup(self.selectedGroup());
                toastr.success("Đã xóa dữ liệu", "Thành công!");
            },
            error: function () {
                toastr.error("Đã có lỗi", "Thất bại!");
            }
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
        $('#ghiLai').modal('show');
        self.selectedDevice(item);
       
    }
    self.update = function (item) {
        var id = item.ID();
        $.ajax({
            url: '/api/device/' + id,
            data: ko.mapping.toJSON(item),
            type: 'PUT',
            contentType: 'application/json',
            dataType: 'json',           
            
        }).done( function (data) {
            self.loadDeviceByGroup(self.selectedGroup());
            toastr.success("Đã sửa dữ liệu", "Thành công!");
            $('#ghiLai').modal('hide');
        });
        $('#ghiLai').modal('hide');
    }

    function getGroupModel(data) {
        var items = getNestedGroup(0, data);
        //<remove duplicates, for infinity nesting only>   
        for (var i = 0; i < items.length; i++) {
            if (items[i].used) {
                items.splice(i, 1);
                i--;
            }
        }
        //</remove duplicates, for infinity nesting only>
        //<build root item>
        return items;
    };
    function getNestedGroup(index, all) {
        var root = all[index];
        if (!root) {
            return all;
        }
        if (!all[index].children) {
            all[index].children = [];
        }
        for (var i = 0; i < all.length; i++) {
            //<infinity nesting?>
            //put children inside it's parent
            if (all[index].id == all[i].parentID) {
                all[index].children.push(all[i]);
                all[i].used = true;
            }
            //</infinity nesting?>
        }
        //all[index].order = index;
        return getNestedGroup(++index, all);
    };
    $(function () {
        var connection = $.hubConnection("https://iot.tek4tv.vn");
        var hub = connection.createHubProxy("echo");
        hub.on("AddMessage", Method);
        connection.start({ jsonp: true })
            .done(function () {
                console.log('connected');
                hub.say("success?");
            })
            .fail(function (a) {
                console.log('not connected' + a);
            });
    });

    function Method(messageFromHub) {
        alert(messageFromHub);
    }
}

$(function () {
    var deviceModel = new DeviceModel();  

    deviceModel.getCategories();

    ko.applyBindings(deviceModel);
});

