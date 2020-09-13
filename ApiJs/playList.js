var PlaylistModel = function () {
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

    self.playlists = ko.observableArray();
    self.getPlaylists= function () {
        $.ajax({
            url: '/api/playlist/all',
            type: 'GET'
        }).done(function (items) {
            self.playlists.removeAll();
            $.each(items, function (index, item) {
                self.playlists.push(self.convertToKoObject(item))
            });
           
        });
    }

    self.selectedPlaylist = ko.observable();
    self.showPlaylistInfo = function () {
        $.ajax({
            url: '/api/playlist/info',
            type: 'GET'
        }).done(function (data) {
            self.selectedPlaylist(data)
        });
    }

    self.showModel = function () {
        self.mode('create');
        self.showPlaylistInfo();
        $('#ghiLai').modal('show');
    }

    self.create = function (item) {
        $.ajax({
            url: '/api/playlist/add',
            type: 'POST',
            data: ko.mapping.toJSON(item),
            contentType: 'application/json',
            dataType: 'json',
            success: function (data) {
                self.getPlaylists();
                $('#ghiLai').modal('hide');
                toastr.success("Đã thêm mới dữ liệu", "Thành công!");
            },
            error: function () {
                toastr.success("Đã có lỗi", "Thất bại!");
            }
                
        });
    }

    self.remove = function (item) {
        var id = item.ID();
        $.ajax({
            url: '/api/playlist/' + id,
            type: 'DELETE',
            contentType: 'application/json',
            dataType: 'json',
            success: function (data) {
                self.getPlaylists();
                toastr.success("Đã xóa dữ liệu", "Thành công!");
            },
            error: function () {
                toastr.success("Đã có lỗi", "Thất bại!");
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
        console.log(item)
        self.mode('update');
        self.selectedPlaylist(item);
        $('#ghiLai').modal('show');
    }

    self.update = function (item) {
        $.ajax({
            url: '/api/playlist/' + item.ID(),
            type: 'PUT',
            data: ko.mapping.toJSON(item),
            contentType: 'application/json',
            dataType: 'json',
            success: function (data) {
                self.getPlaylists();
                $('#ghiLai').modal('hide');
                toastr.success("Đã sửa dữ liệu", "Thành công!");
            },
            error: function () {
                toastr.success("Đã có lỗi", "Thất bại!");
            }
        });
    }

    self.events = ko.observableArray();
    self.loadDetail = function (item) {
        $.getJSON("https://dev.tek4tv.vn/api/device/playlist/1?=IwAR1TFpgkPs7XrWfnPgUle3O45uQbcVac9G-BLLCiHbgfM8sRbM-7iyPBU4s",
            function (data) {
                console.log(data);
            });
       // $('#loading').modal('show');
       // self.events.removeAll();
       /* $.getJSON('https://dev.tek4tv.vn/api/device/playlist/1?fbclid=IwAR1TFpgkPs7XrWfnPgUle3O45uQbcVac9G-BLLCiHbgfM8sRbM-7iyPBU4s', function (data) {
            console.log(data);*/
           /* $.each(data, function (idx, item) {
                item.Edit = false;
                self.events.push(self.convertToKoObject(item))
            });*/
           // self.events(self.events().sort(function (l, r) { return l.Index() - r.Index() }));
           // $('#loading').modal('hide');
       // })   
    }
    
    self.loadPlaylistById = function (item) {
        self.selectedPlaylist(item);
        self.loadDetail(self.selectedPlaylist());
       
    }
}
$(function () {
    var playlistModel = new PlaylistModel();
    playlistModel.getPlaylists();
    ko.applyBindings(playlistModel);
});