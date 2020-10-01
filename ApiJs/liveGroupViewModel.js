var GroupPlaylistModel = function () {
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

    // live group
    self.selectedGroup = ko.observable();
    self.groups = ko.observableArray();

    self.getGroupAll = function () {
        $.ajax({
            url: '/api/group/all',
            type: 'GET'
        }).done(function (data) {
            self.groups.removeAll();
            $.each(data, function (index, item) {
                self.groups.push(self.convertToKoObject(item))
            })
        });
    }
    self.showGroupInfo = function () {
        $.ajax({
            url: '/api/group/info',
            type: 'GET'
        }).done(function (data) {
            self.selectedGroup(data)
        });
    }

    self.showModelGroup = function () {
        self.mode('createGroup');
        self.showGroupInfo();
        $('#ghilai-group').modal('show');
    }

    self.createGroup = function (item) {
        item.IsShow = true;
        $.ajax({
            url: '/api/group/add',
            type: 'POST',
            data: ko.mapping.toJSON(item),
            contentType: 'application/json',
            dataType: 'json',
            success: function (data) {
                self.getGroupAll();
                $('#ghilai-group').modal('hide');
                toastr.success("Đã thêm mới dữ liệu", "Thành công!");
            },
            error: function () {
                toastr.error("Đã có lỗi", "Thất bại!");
            }

        }).success();
    }

    self.removeGroup = function (item) {
        var id = item.ID();
        $.ajax({
            url: '/api/group/' + id,
            type: 'DELETE',
            contentType: 'application/json',
            dataType: 'json',
            success: function (data) {
                self.getGroupAll();
                toastr.success("Đã xóa dữ liệu", "Thành công!");
            },
            error: function () {
                toastr.error("Đã có lỗi", "Thất bại!");
            }
        }).success();
    }
    self.confirmRemoveGroup = function (item) {
        var result = confirm("Bạn muốn xóa chứ ??");
        if (result) {
            self.removeGroup(item);
        }
    }

    self.editGroup = function (item) {
        self.mode('updateGroup');
        self.selectedGroup(item);
        $('#ghilai-group').modal('show');
    }

    self.update = function (item) {
        $.ajax({
            url: '/api/group/' + item.ID(),
            type: 'PUT',
            data: ko.mapping.toJSON(item),
            contentType: 'application/json',
            dataType: 'json',
            success: function (data) {
                self.getGroupAll();
                $('#ghilai-group').modal('hide');
                toastr.success("Đã sửa dữ liệu", "Thành công!");
            },
            error: function () {
                toastr.error("Đã có lỗi", "Thất bại!");
            }
        });
    }





    self.playlistGroups = ko.observableArray();

    self.getTreeGroups = function () {
        $.ajax({
            url: '/api/group/all',
            type: 'GET'
        }).done(function (items) {
            //  self.categories.removeAll();
            $.each(items, function (index, item) {
                self.playlistGroups.push(self.convertToKoObject(item))
            });
            self.createTreeGroup();
        });
    }

    self.selectedPlaylistGroup = ko.observable();

    self.createTreeGroup = function () {
        var data = [];
        $.each(self.playlistGroups(), function (idx, item) {
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
                self.selectedPlaylistByGroup(self.convertToKoObject(gr));
                self.loadPlaylistByGroup(self.selectedPlaylistByGroup());

                //các nguồn tiếp sóng: IP, FM, AM cho từng cụm thu / phát
                self.loadInputSource(self.convertToKoObject(gr))
                console.log(gr)
            }
        );
    }
    // Thiết bị
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
    self.showDeviceByGroup = function () {
        $('#save-DeviceByGroup').modal('show');
    }
    self.devicesAdded = ko.observableArray();
    self.getAllDevices = function () {
        $.ajax({
            url: '/api/device/all',
            type: 'GET'
        }).done(function (data) {
            self.devicesAdded.removeAll();
            $.each(data, function (index, item) {
                self.devicesAdded.push(self.convertToKoObject(item))
            })
        });
    }
    self.selectedDevice = ko.observable();
    self.showInfo = function () {
        $.ajax({
            url: '/api/device/info',
            type: 'GET'
        }).done(function (data) {
            self.selectedDevice(data);
        });
    }
    self.removeDevice = function (item) {
        var idGroup = self.selectedDeviceByGroup().ID();
        var idDevice = item.ID();
        $.ajax({
            url: '/api/group/' + idGroup + '/device/' + idDevice,
            type: 'DELETE',
            contentType: 'application/json',
            dataType: 'json',
            success: function (data) {
                self.loadDeviceByGroup(self.selectedDeviceByGroup());
                toastr.success("Đã xóa dữ liệu", "Thành công!");
            },
            error: function () {
                toastr.error("Đã có lỗi", "Thất bại!");
            }
        });
    }
    self.confirmRemoveDevice = function (item) {
        var result = confirm("Bạn muốn xóa chứ ??");
        if (result) {
            self.removeDevice(item);
        }
    }
    self.addDevice = function (item) {
        var idGroup = self.selectedDeviceByGroup().ID();
        var idDevice = item.ID();
        $.ajax({
            url: '/api/group/' + idGroup + '/device/' + idDevice,
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            success: function (data) {
                self.loadDeviceByGroup(self.selectedDeviceByGroup());
                $('#save-DeviceByGroup').modal('hide');
                toastr.success("Đã thêm mới dữ liệu", "Thành công!");
            },
            error: function () {
                toastr.error("Đã có lỗi", "Thất bại!");
                $('#save-DeviceByGroup').modal('hide');
            }
        });
    }
    self.confirmAddDevice = function (item) {
        var result = confirm("Bạn muốn thêm chứ ??");
        if (result) {
            self.addDevice(item);
            self.loadDeviceByGroup(self.selectedDeviceByGroup());
        }
    }

    // lịch phát sóng
    self.selectedPlaylistByGroup = ko.observable();
    self.getPlaylistByGroup = ko.observableArray();
    self.loadPlaylistByGroup = function (item) {
        $.ajax({
            url: '/api/group/playlist/' + self.convertToJson(objToken)[0].Name + "/" + item.ID(),
            type: 'GET',
            contentType: 'application/json',
            dataType: 'json'
        }).success(function (data) {
            self.getPlaylistByGroup.removeAll();
            $.each(data, function (index, item) {
                self.getPlaylistByGroup.push(self.convertToKoObject(item))
            });

        })
    }

    self.removePlaylist = function (item) {
        var idGroup = self.selectedPlaylistByGroup().ID();
        var idPlaylist = item.ID();
        $.ajax({
            url: '/api/group/' + idGroup + '/playlist/' + idPlaylist,
            type: 'DELETE',
            contentType: 'application/json',
            dataType: 'json',
            success: function (data) {
                self.loadPlaylistByGroup(self.selectedPlaylistByGroup());
                toastr.success("Đã xóa dữ liệu", "Thành công!");
            },
            error: function () {
                toastr.error("Đã có lỗi", "Thất bại!");
            }
        });
    }
    self.confirmRemovePlaylist = function (item) {
        var result = confirm("Bạn muốn xóa chứ ??");
        if (result) {
            self.removePlaylist(item);
        }
    }
    self.showModelPlaylist = function () {
        $('#save-playlistByGroup').modal('show');
    }
    var objToken = $('#decodeToken').val();
    var user = $('#user').val();
    self.playlists = ko.observableArray();

    self.getPlaylists = function () {
        $.ajax({
            url: '/api/playlist/all/' + self.convertToJson(objToken)[0].Name,
            type: 'GET'
        }).done(function (items) {
            self.playlists.removeAll();
            $.each(items, function (index, item) {
                self.playlists.push(self.convertToKoObject(item))
            });

        });
    }
    self.addPlaylist = function (item) {
        var idGroup = self.selectedPlaylistByGroup().ID();
        var idPlaylist = item.ID();
        $.ajax({
            url: '/api/group/' + idGroup + '/playlist/' + idPlaylist,
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            success: function (data) {
                $('#save-playlistByGroup').modal('hide');
                toastr.success("Đã thêm mới dữ liệu", "Thành công!");
                self.loadPlaylistByGroup(self.selectedPlaylistByGroup());
            },
            error: function () {
                toastr.error("Đã có lỗi", "Thất bại!");
                $('#save-playlistByGroup').modal('hide');
            }
        });

    }
    self.confirmAddPlaylist = function (item) {
        var result = confirm("Bạn muốn thêm chứ ??");
        if (result) {
            self.addPlaylist(item);
        }
    }

    // searching devices
    self.keySearch = ko.observable();
    self.resultAutoSearch = ko.observableArray();
    self.autoSearchDevice = function () {
        var valueString = self.keySearch();

        $.ajax({
            url: "/api/device/search/" + valueString,
            type: 'get',
            success: function (data) {
                self.resultAutoSearch.removeAll();
                $.each(data, function (index, item) {
                    self.resultAutoSearch.push(self.convertToKoObject(item))
                })
            }
        });
    }
    self.enterSearch = function (d, e) {
        e.keyCode === 13 && self.autoSearchDevice();
        return true;
    };


    // searching playlist
    self.keySearchPlaylist = ko.observable();
    self.resultAutoSearchPlaylist = ko.observableArray();
    self.autoSearchPlaylist = function () {
        var valueString = self.keySearchPlaylist();

        $.ajax({
            url: "/api/playlist/search/" + self.convertToJson(objToken)[0].Name + "/" + valueString,
            type: 'get',
            success: function (data) {
                self.resultAutoSearchPlaylist.removeAll();
                $.each(data, function (index, item) {
                    self.resultAutoSearchPlaylist.push(self.convertToKoObject(item))
                })

            }
        });
    }
    self.enterSearchPlaylist = function (d, e) {
        e.keyCode === 13 && self.autoSearchPlaylist();
        return true;
    };

    // các nguồn tiếp sóng: IP, FM, AM cho từng cụm thu/phát


    var typeName = function (name) {
        this.name = name;
    };
    self.viewTypeModel = {
        type: ko.observableArray(["Tuner", "IP"]),
        selectType: ko.observable()
    }
    self.valueInputSoure = ko.observable();
    self.valueInputSoures = ko.observableArray();
    self.addInputSource = function () {
        var obj = {
            Name: "ten",
            Type: "",
            Param: "0",
            IsSchedule: "",
            Start: "00:00:00",
            End: "00:00:00"
        };
        self.valueInputSoure(self.convertToKoObject(obj));
        self.mode('createInputSource');
        $('#save-inputSource').modal('show');
    }

    self.saveInputSource = function (item) {
        self.valueInputSoures.unshift(self.valueInputSoure())
    }
    self.removeInputSource = function (item) {
        self.valueInputSoures.remove(item);
    }
    self.editInputSource = function (item) {
        self.valueInputSoure(item);
        self.mode('updateInputSource');
        $('#save-inputSource').modal('show');
    }
    self.addNewInputSource = function (item) {
        var id = item.ID();
        var data = [];
        $.each(self.valueInputSoures(), function (i, obj) {
            data.push({
                Name: obj.Name(),
                Type: obj.Type(),
                Param: obj.Param(),
                IsSchedule: obj.IsSchedule(),
                Start: obj.Start,
                End: obj.End
            });
        })
        var payload = { "InputSource": ko.toJSON(data) }
        console.log(ko.mapping.toJSON(payload))
        $.ajax({
            url: "/api/group/add/InputSource/" + id,
            type: 'PUT',
            data: ko.mapping.toJSON(payload),
            contentType: 'application/json',
            dataType: 'json',
            success: function (data) {
                toastr.success("Đã thêm playlist", "Thành công");
            }
        })

    }
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

        });
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
    var groupPlaylistModel = new GroupPlaylistModel();
    groupPlaylistModel.getGroupAll();
    groupPlaylistModel.getPlaylists();
    groupPlaylistModel.getTreeGroups();
    groupPlaylistModel.getAllDevices();
    ko.applyBindings(groupPlaylistModel);
});