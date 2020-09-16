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
        item.IsDelete = false
        item.IsPublish = true
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
        self.loadAddedPlaylist(item)
    }
    self.modalAddPlaylist = function () {  
        $('#addPlaylist').modal('show');
        self.loadIotCat();
    }
    self.valueLive = ko.observable();
    self.valueLives = ko.observableArray();
    self.modalAddLive = function () {      
        var obj = {
            Category: { ID: - 1, Name: 'Live' },
            Duration: "02:00:00",
            Edit: true,
            Start: "00:00:00",
            End: "00:00:00",
            ID: -1,
            Index: 0,
            Name: "Live stream input",
            Path: "rtmp://...",
        };
        self.valueLive(self.convertToKoObject(obj));  
        self.mode('addLive');
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
    self.addNewPlaylist = function (item) {
        var id = item.ID();
        self.valueLives(self.valueLives().sort(function (l, r) { return l.Index() - r.Index() }));
        var data = [];
        $.each(self.valueLives(), function (i, obj) {
            data.push({
                Index: obj.Index, Category: { ID: obj.Category.ID(), Name: obj.Category.Name() }, ID: obj.ID(), Name: obj.Name, Duration: obj.Duration(), Path: obj.Path(), Start: obj.Start, End: obj.End, Edit: false
            });
        })        
        var payload = { ID: item.ID(), Playlist: ko.toJSON(data)};       
        item.Playlist = JSON.stringify(payload);     
          $.ajax({
              url: "/api/playlist/"+id,
              type: 'PUT',
              data: ko.mapping.toJSON(item),
              contentType: 'application/json',
              dataType: 'json',
              success: function (data) {
                    toastr.success("Đã thêm playlist", "Thành công");
              },
              error: function () {
                    toastr.error("Không thêm thành công", "Thất bại");
                }
          })   
    }
    self.loadAddedPlaylist = function (item) {     
        $.ajax({
            url: "/api/playlist/" + item.ID(),
            type: 'GET'
        }).done(function (data) {
            self.valueLives.removeAll();
            var items = self.convertToJson(self.convertToJson(data[0].Playlist).Playlist);
            $.each(items, function (index, item) {
                self.valueLives.push(self.convertToKoObject(item))
            }); 
           
        });
    }


    // cal api iot
    self.listIotCats = ko.observableArray();
    self.loadIotCat = function () {
        $.ajax({
            url: "/api/iot/category",
            type: "get"
        }).done(function (items) {
            self.listIotCats.removeAll();
            $.each(items, function (index, item) {
                self.listIotCats.push(self.convertToKoObject(item))
            });          
            self.createTreeCat();           
            
            
        });
    }

    self.selectdCat = ko.observable();
    self.videoId = ko.observableArray();
    self.createTreeCat = function () {
        var data = [];
        $.each(self.listIotCats(), function (idx, item) {
            data.push({ id: item.ID(), text: item.Name(), parentID: item.ParentID() });
        });
        
        $('#treeCat').jstree("destroy");
        $("#treeCat").jstree({
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

        $('#treeCat').on('ready.jstree', function () {
            $("#treeCat").jstree("open_all");
        });

        $("#treeCat").on("select_node.jstree",
            function (evt, data) {
                var nodeId = $('#treeCat').jstree().get_selected("id")[0].id;
                var nodeName = $('#treeCat').jstree().get_selected("id")[0].text;               
                var gr = { ID: nodeId, Name: nodeName };              
                self.selectdCat(self.convertToKoObject(gr));                          
                self.videoId.removeAll(); 
               
                $.each(self.listIotCats(), function (index, item) {
                    if (gr.ID == item.ParentID()) {                        
                        self.videoId.push(item.ID())
                    }                 
                });              
                $.each(self.listIotCats(), function (index1, item1) {
                    $.each(self.videoId(), function (index2, item2) {
                        if (item2 == item1.ParentID()) {
                            self.videoId.push(item1.ID())
                        } 
                     });
                });
                self.videoId.push(gr.ID);
             
                self.loadVideoByCat(self.videoId())
            }
        );
    }

    self.Projects = ko.observableArray()
    self.loadVideoByCat = function (item) {            
        var payload = { ListCategory: item, Page: 0, Size: 40 }
      
        $.ajax({
            url: '/api/iot/video' ,
            type: 'POST',
            data: ko.mapping.toJSON(payload),
            contentType: 'application/json',
            dataType: 'json'
        }).success(function (items) {
            self.Projects.removeAll();
            $.each(items.Projects, function (index, item) {
                self.Projects.push(self.convertToKoObject(item))
            }); 
           
        })
    }


    self.isImageStorage = function (item) {
        if (item.ThumbNail() === null || item.ThumbNail().length === 0) {
            return '/BackEnd/assets/global/img/no_thumb.jpg';
        }
        var ext = item.ThumbNail().split('.').pop().toLowerCase();
        var fileExtension = ['jpeg', 'jpg', 'png', 'gif', 'bmp'];
        if ($.inArray(ext, fileExtension, 0) === -1) {
            return '/BackEnd/assets/global/img/no_thumb.jpg';
        } else {
            var listRemote = item.RemoteStorage().split('#');
            var url = "https://storage.tek4tv.vn";
            return url + "/" + item.ThumbNail();
        }
    };

    self.isImageThumb = function (path) {
        if (path == null || path.length == 0) {
            return '/BackEnd/assets/global/img/no_thumb.jpg';
        }
        var ext = path.split('.').pop().toLowerCase();
        var fileExtension = ['jpeg', 'jpg', 'png', 'gif', 'bmp']
        if ($.inArray(ext, fileExtension, 0) == -1) {
            return '/BackEnd/assets/global/img/no_thumb.jpg';
        } else {
            return 'Storage/' + path;
        }
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
    var playlistModel = new PlaylistModel();
    playlistModel.getPlaylists();
    ko.applyBindings(playlistModel);
});