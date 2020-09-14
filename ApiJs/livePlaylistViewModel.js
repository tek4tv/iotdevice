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

    self.loadPlaylistById = function (item) {
        self.selectedPlaylist(item);    
    }

    self.modalAddPlaylist = function () {       
        $('#addPlaylist').modal('show');
    }

    self.valueLive = ko.observable();
    self.valueLives = ko.observableArray();
    self.modalAddLive = function () {
        self.mode('addLive');
        var obj = {
            Category: { ID: - 1, Name: 'Live' },
            Duration: "02:00:00",
            Edit: true,
            Start: "00:00:00",
            End: "00:00:00",
            ID: 0,
            Index: 0,
            Name: "Live stream input",
            Path: "rtmp://...",
        };
        self.valueLive(self.convertToKoObject(obj));     
        $('#addLive').modal('show');
    }
    self.savePlaylist = function (item) {
        self.valueLives.unshift(self.valueLive())
        console.log(self.valueLives())
    }
    self.editEvent = function (item) {
        console.log(item);
        console.log('ok');
       self.valueLive(item);
       self.mode('editLive');
       $('#addLive').modal('show');
    }
    self.removeEvent = function (item) {
        self.valueLives.remove(item);
    }
}
$(function () {
    var playlistModel = new PlaylistModel();
    playlistModel.getPlaylists();
    ko.applyBindings(playlistModel);
});