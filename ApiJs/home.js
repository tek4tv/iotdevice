var HomeModel = function () {
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

    self.groups = ko.observableArray();
    self.getTreeGroups = function () {
        $.ajax({
            url: '/api/group/all',
            type: 'GET'
        }).done(function (items) {
            self.groups.removeAll();
            $.each(items, function (index, item) {
                self.groups.push(self.convertToKoObject(item))
            });
            self.createTreeGroup();
        });
    }
    self.createTreeGroup = function () {
        var data = [];
        $.each(self.groups(), function (idx, item) {
            data.push({ id: item.ID(), text: item.Name(), parentID: item.ParentID() });
        });
        $("#tree_group0").jstree({
            "core": {
                "themes": {
                    "responsive": false
                },
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

        $('#tree_group0').on('ready.jstree', function () {
            $("#tree_group0").jstree("open_all");
        });

        $("#tree_group0").on("select_node.jstree",
            function (evt, data) {
                var nodeId = $('#tree_group0').jstree().get_selected("id")[0].id;
                var nodeName = $('#tree_group0').jstree().get_selected("id")[0].text;
                var gr = { ID: nodeId, NAME: nodeName };

                self.selectedDeviceByGroup(self.convertToKoObject(gr));
                self.loadDeviceByGroup(self.selectedDeviceByGroup());

               /* self.selectedPlaylistByGroup(self.convertToKoObject(gr));
                self.loadPlaylistByGroup(self.selectedPlaylistByGroup());*/

                //các nguồn tiếp sóng: IP, FM, AM cho từng cụm thu / phát
                self.loadInputSource(self.convertToKoObject(gr))

                self.getPlaylistByGroupName(gr)
                
               
            }
        );
    }

    self.selectedDeviceByGroup = ko.observable();
    self.getDeviceByGroup = ko.observableArray();
    self.loadDeviceByGroup = function (item) {
        $.ajax({
            url: '/api/group/device/' + item.ID(),
            type: 'GET',
            contentType: 'application/json',
            dataType: 'json'
        }).success(function (data) {
            
            self.getDeviceByGroup.removeAll();
            $.each(data, function (index, item) {
                self.getDeviceByGroup.push(self.convertToKoObject(item))
            });
        })     
    }
    
    self.playlistByGroupNames = ko.observableArray();
    self.getPlaylistByGroupName = function (item) {    
        var name = { ID: item.ID };
        console.log(name)
        $.ajax({
            url: '/api/group/playlist' ,
            type: 'POST',
            data: ko.mapping.toJSON(name),
            contentType: 'application/json',
            dataType: 'json',
        }).success(function (data) {
            self.playlistByGroupNames.removeAll();
            $.each(data, function (index, item) {
                self.playlistByGroupNames.push(self.convertToKoObject(item))
            });
            console.log(self.playlistByGroupNames())
        })
    }  

    self.valueInputSoures = ko.observableArray();
    self.loadInputSource = function (item) {
        $.ajax({
            url: "/api/group/" + item.ID(),
            type: 'GET'
        }).done(function (data) {
            self.valueInputSoures.removeAll();
            var items = self.convertToJson(data[0].InputSource);
            $.each(items, function (index, item) {
                self.valueInputSoures.push(self.convertToKoObject(item))
            })
            console.log(self.valueInputSoures())
        });
    }

    //get playlist by imei
    self.loadPlaylistByImei = function (item) {
        console.log(item.IMEI())
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
}
$(function () {
    var homeModel = new HomeModel();
    homeModel.getTreeGroups();
    ko.applyBindings(homeModel);
});