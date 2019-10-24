var RegionController = RegionController || {};

RegionController = {
    model: {
        locations: [],
        defaultDistrict: 0,
        timerid: 0,
        districtSelected: 0,
    },
    init: function () {
        // Initialize event
        RegionController.events.getCity();
        $("#CitySelectedValue").on('change', RegionController.events.getDistrict);
        $("#DistrictSelectedValue").on('change', RegionController.events.getBranch);
        $("input[name='address']").keyup(RegionController.events.delay);
        $("input[name='radio']").change(RegionController.events.delay);
    },
    initAdmin: function () {
        // Register event
        RegionController.model.defaultDistrict = "";
        $("#CitySelectedValue").on('change', RegionController.events.getDistrict);
        // Initialize event
        $("#CitySelectedValue").trigger('change');
    },
    events: {
        getCity: function () {
            $.ajax({
                url: "/get-city",
                type: 'GET',
                contentType: "application/json; charset=utf-8",
                cache: false,
                beforeSend: function () {
                    $("#CitySelectedValue").attr("disabled", true);
                    $("#CitySelectedValue").html('<option>Đang tải...</option>')
                },
                success: function (response) {
                    if (response.Code == 200) {
                        $("#CitySelectedValue").attr("disabled", false);
                        $("#CitySelectedValue").children().remove();
                        $("#CitySelectedValue").append($('<option></option>').val(RegionController.model.defaultDistrict).html('Chọn Tỉnh - Thành phố'));
                        $.each(response.Result, function (i, item) {
                            $("#CitySelectedValue").append($('<option></option>').val(item.Value).html(item.Text).attr('selected', item.Selected));
                        });
                        // trigger to get district
                        $("#CitySelectedValue").trigger('change');
                    }
                    else {
                        alert(response.Message);
                    }
                }
            })
        },
        getDistrict: function () {
            $.ajax({
                url: "/get-district",
                type: 'GET',
                contentType: "application/json; charset=utf-8",
                data: { cityId: Number($("#CitySelectedValue").val()), id: Number(RegionController.model.districtSelected) },
                cache: false,
                beforeSend: function () {
                    $("#DistrictSelectedValue").attr("disabled", true);
                    $("#DistrictSelectedValue").html('<option>Đang tải...</option>')
                },
                success: function (response) {
                    if (response.Code == 200) {
                        $("#DistrictSelectedValue").attr("disabled", false);
                        $("#DistrictSelectedValue").children().remove();
                        $("#DistrictSelectedValue").append($('<option></option>').val(RegionController.model.defaultDistrict).html('Chọn Quận - Huyện'));
                        $.each(response.Result, function (i, item) {
                            $("#DistrictSelectedValue").append($('<option></option>').val(item.Value).html(item.Text).attr('selected', item.Selected));
                        });

                        $("#DistrictSelectedValue").trigger("change");
                    }
                    else {
                        alert(response.Message);
                    }
                }
            })
        },
        getBranch: function () {
            var checkbox = [];
            $("input[name='radio']:checked").each(function () {
                checkbox.push(Number($(this).val()));
            })

            $.ajax({
                url: "/get-branch",
                type: 'GET',
                contentType: "application/json; charset=utf-8",
                traditional: true,
                data: {
                    cityId: Number($("#CitySelectedValue").val()),
                    id: Number($("#DistrictSelectedValue").val()),
                    oilType: checkbox,
                    address: $("input[name='address']").val()
                },
                cache: false,
                beforeSend: function () {

                },
                success: function (response) {
                    if (response.Code == 200) {
                        var str = "";
                        var locations = [];
                        $.each(response.Result, function (i, item) {
                            str += '<li class="item" onClick="myClick(' + i + ')"><a>'
                            + '<p class= "name">' + item.OfficeName + '</p>'
                            + '<p class="address">' + item.Address + '</p>'
                            + '<p class="phone">' + item.Phone + '</p>'
                            + '</a></li>';
                            // Get location
                            locations.push({
                                lat: item.Lat,
                                lng: item.Lng,
                                name: item.OfficeName,
                                address: item.Address,
                                phone: item.Phone,
                                image: "/Content/resources_new/img/icons/address.png"
                            });
                        });
                        $(".list-address").html(str);
                        inputLocations = locations;
                        markers = [];
                        initialize();
                    }
                    else {
                        alert(response.Message);
                    }
                }
            })
        },
        delay: function () {
            clearTimeout(RegionController.model.timerid);
            RegionController.model.timerid = setTimeout(RegionController.events.getBranch, 1000);
        }
    }
}