$(document).ready(function () {

    // Dodawanie nowego woluminu w sekcji głównej wolumenów
    $('#add-volume').on('click', function () {
        let volumeIndex = $('#volumes-container .volume-item').length;
        let newVolume = `<div class="volume-item" data-index="${volumeIndex}">
                            <h4>Volume ${volumeIndex}</h4>
                            <div class="form-group">
                                <label for="Volumes_${volumeIndex}__Name">Name</label>
                                <input id="Volumes_${volumeIndex}__Name" name="Volumes[${volumeIndex}].Name" class="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="Volumes_${volumeIndex}__Driver">Driver</label>
                                <input id="Volumes_${volumeIndex}__Driver" name="Volumes[${volumeIndex}].Driver" class="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Driver Options</label>
                                <div id="driver-options-container-${volumeIndex}" class="driver-options-container"></div>
                                <button type="button" class="btn btn-secondary add-driver-option-btn" data-volume-index="${volumeIndex}">Add Driver Option</button>
                            </div>
                            <div class="form-group">
                                <label>Labels</label>
                                <div id="labels-container-${volumeIndex}" class="labels-container"></div>
                                <button type="button" class="btn btn-secondary add-label-btn" data-volume-index="${volumeIndex}">Add Label</button>
                            </div>
                            <div class="form-group">
                                <label>External</label>
                                <input type="checkbox" name="Volumes[${volumeIndex}].External" value="true" />
                                <input type="hidden" name="Volumes[${volumeIndex}].External" value="false" />
                            </div>
                            <div class="form-group">
                                <label>Read-Only</label>
                                <input type="checkbox" name="Volumes[${volumeIndex}].ReadOnly" value="true" />
                                <input type="hidden" name="Volumes[${volumeIndex}].ReadOnly" value="false" />
                            </div>
                            <button type="button" class="btn btn-danger remove-volume-btn">Remove Volume</button>
                        </div>`;
        $('#volumes-container').append(newVolume);
    });

    // Dodawanie woluminu tylko z polem Name w sekcji usług
    $('#services-container').on('click', '.add-volume-btn', function () {
        const serviceIndex = $(this).data('service-index');
        const volumesContainer = $(`#volumes-container-${serviceIndex}`);
        const volumeIndex = volumesContainer.children('.volume').length;

        let volumeDiv = `<div class="volume">
                            <input type="text" name="Services[${serviceIndex}].Volumes[${volumeIndex}]" placeholder="Volume Name" class="form-control" />
                            <button type="button" class="remove-volume-btn btn btn-danger">Remove Volume</button>
                        </div>`;
        volumesContainer.append(volumeDiv);
    });

    // Usuwanie woluminu w sekcji głównej wolumenów
    $('#volumes-container').on('click', '.remove-volume-btn', function () {
        $(this).closest('.volume-item').remove();
    });

    // Usuwanie woluminu w sekcji usług
    $('#services-container').on('click', '.remove-volume-btn', function () {
        $(this).closest('.volume').remove();
    });
    // Dodawanie Driver Option
    $('#volumes-container').on('click', '.add-driver-option-btn', function () {
        const volumeIndex = $(this).data('volume-index');
        const driverOptionsContainer = $(`#driver-options-container-${volumeIndex}`);
        const optionIndex = driverOptionsContainer.children().length;

        let driverOptionDiv = `<div class="driver-option">
                                   <input type="text" name="Volumes[${volumeIndex}].DriverOptions[${optionIndex}].Key" placeholder="Key" class="form-control" />
                                   <input type="text" name="Volumes[${volumeIndex}].DriverOptions[${optionIndex}].Value" placeholder="Value" class="form-control" />
                                   <button type="button" class="btn btn-danger remove-driver-btn">Remove Driver Option</button>
                               </div>`;
        driverOptionsContainer.append(driverOptionDiv);
    });

    // Dodawanie Label
    $('#volumes-container').on('click', '.add-label-btn', function () {
        const volumeIndex = $(this).data('volume-index');
        const labelsContainer = $(`#labels-container-${volumeIndex}`);
        const labelIndex = labelsContainer.children().length;

        let labelDiv = `<div class="label">
                            <input type="text" name="Volumes[${volumeIndex}].Labels[${labelIndex}].Key" placeholder="Label Key" class="form-control" />
                            <input type="text" name="Volumes[${volumeIndex}].Labels[${labelIndex}].Value" placeholder="Label Value" class="form-control" />
                            <button type="button" class="btn btn-danger remove-label-btn">Remove Label</button>
                        </div>`;
        labelsContainer.append(labelDiv);
    });

    // Usuwanie woluminu
    $('#volumes-container').on('click', '.remove-volume-btn', function () {
        $(this).closest('.volume-item').remove();
    });

    // Usuwanie Driver Option
    $('#volumes-container').on('click', '.remove-driver-btn', function () {
        $(this).closest('.driver-option').remove();
    });

    // Usuwanie Label
    $('#volumes-container').on('click', '.remove-label-btn', function () {
        $(this).closest('.label').remove();
    });

    $('#add-network').on('click', function () {
        let networkIndex = $('#networks-container .network-item').length;
        let newNetwork = `<div class="network-item" data-index="${networkIndex}">
                                            <h4>Network ${networkIndex}</h4>
                                            <div class="form-group">
                                                <label for="Networks_${networkIndex}__Name">Name</label>
                                                <input id="Networks_${networkIndex}__Name" name="Networks[${networkIndex}].Name" class="form-control" />
                                            </div>
                                            <div class="form-group">
                                                <label for="Networks_${networkIndex}__Driver">Driver</label>
                                                <input id="Networks_${networkIndex}__Driver" name="Networks[${networkIndex}].Driver" class="form-control" />
                                            </div>
                                            <div class="form-group">
                                                <label>IPAM Driver</label>
                                                <input type="text" name="Networks[${networkIndex}].Ipam.Driver" class="form-control" />
                                            </div>
                                            <div class="form-group">
                                                <label>IPAM Configuration</label>
                                                <div id="ipam-config-container-${networkIndex}"></div>
                                                <button type="button" class="btn btn-secondary add-ipam-config-btn" data-network-index="${networkIndex}">Add IPAM Config</button>
                                            </div>
                                            <div class="form-group">
                                                <label>Driver Options</label>
                                                <div id="driver-options-container-${networkIndex}"></div>
                                                <button type="button" class="btn btn-secondary add-driver-option-btn" data-network-index="${networkIndex}">Add Driver Option</button>
                                            </div>
                                            <div class="form-group">
                                                <label>Internal</label>
                                                <input type="checkbox" name="Networks[${networkIndex}].Internal" value="true" />
                                                <input type="hidden" name="Networks[${networkIndex}].Internal" value="false" />
                                            </div>
                                            <div class="form-group">
                                                <label>Attachable</label>
                                                <input type="checkbox" name="Networks[${networkIndex}].Attachable" value="true" />
                                                <input type="hidden" name="Networks[${networkIndex}].Attachable" value="false" />
                                            </div>
                                            <button type="button" class="btn btn-danger remove-network-btn">Remove Network</button>
                                        </div>`;
        $('#networks-container').append(newNetwork);
    });

    // Dodawanie IPAM Configuration
    $('#networks-container').on('click', '.add-ipam-config-btn', function () {
        const networkIndex = $(this).data('network-index');
        const ipamConfigContainer = $(`#ipam-config-container-${networkIndex}`);

        let ipamDiv = `<div class="ipam-config">
                            <input type="text" name="Networks[${networkIndex}].Ipam.Configurations[0].Subnet" placeholder="Subnet" class="form-control" />
                            <input type="text" name="Networks[${networkIndex}].Ipam.Configurations[0].Gateway" placeholder="Gateway" class="form-control" />
                            <button type="button" class="btn btn-danger remove-ipam-btn">Remove IPAM Config</button>
                        </div>`;
        ipamConfigContainer.append(ipamDiv);
    });

    // Dodawanie Driver Option
    $('#networks-container').on('click', '.add-driver-option-btn', function () {
        const networkIndex = $(this).data('network-index');
        const driverOptionsContainer = $(`#driver-options-container-${networkIndex}`);

        let driverOptionDiv = `<div class="driver-option">
                                   <input type="text" name="Networks[${networkIndex}].DriverOptions[0].Key" placeholder="Key" class="form-control" />
                                   <input type="text" name="Networks[${networkIndex}].DriverOptions[0].Value" placeholder="Value" class="form-control" />
                                   <button type="button" class="btn btn-danger remove-driver-btn">Remove Driver Option</button>
                               </div>`;
        driverOptionsContainer.append(driverOptionDiv);
    });

    // Usuwanie sieci
    $('#networks-container').on('click', '.remove-network-btn', function () {
        $(this).closest('.network-item').remove();
    });

    // Usuwanie IPAM Configuration
    $('#networks-container').on('click', '.remove-ipam-btn', function () {
        $(this).closest('.ipam-config').remove();
    });

    // Usuwanie Driver Option
    $('#networks-container').on('click', '.remove-driver-btn', function () {
        $(this).closest('.driver-option').remove();
    });

    $('#add-service').on('click', function () {
        let serviceIndex = $('#services-container .service-item').length;
        let newService = `<div class="service-item" data-index="${serviceIndex}">
                                            <h4>Service ${serviceIndex}</h4>
                                            <div>
                                                <label for="Services_${serviceIndex}__Name">Name</label>
                                                <input id="Services_${serviceIndex}__Name" name="Services[${serviceIndex}].Name" class="form-control" />
                                            </div>
                                            <div>
                                                <label for="Services_${serviceIndex}__Image">Image</label>
                                                <input id="Services_${serviceIndex}__Image" name="Services[${serviceIndex}].Image" class="form-control" />
                                            </div>  
                                           <div class="form-group">
                                            <label>Ports</label>
                                            <div id="ports-container-${serviceIndex}" class="ports-container"></div>
                                            <button type="button" class="add-port-btn btn btn-secondary" data-service-index="${serviceIndex}">Add Port</button>
                                        </div>
                                        <div class="form-group">
                                            <label>Volumes</label>
                                            <div id="volumes-container-${serviceIndex}" class="volumes-container"></div>
                                            <button type="button" class="add-volume-btn btn btn-secondary" data-service-index="${serviceIndex}">Add Volume</button>
                                        </div>
                                        <div class="form-group">
                                            <label>Environment Variables</label>
                                            <div id="environment-container-${serviceIndex}" class="environment-container"></div>
                                            <button type="button" class="add-env-btn btn btn-secondary" data-service-index="${serviceIndex}">Add Environment Variable</button>
                                        </div>
                                        <div class="form-group">
                                            <label>Networks</label>
                                            <div id="networks-container-${serviceIndex}" class="networks-container"></div>
                                            <button type="button" class="add-network-btn btn btn-secondary" data-service-index="${serviceIndex}">Add Network</button>
                                        </div>
                                            <button type="button" class="remove-service btn btn-danger">Remove Service</button>
                                          </div>`;
        $('#services-container').append(newService);
    });

    $('#services-container').on('click', '.add-port-btn', function () {
        const serviceIndex = $(this).data('service-index');
        const portsContainer = $(`#ports-container-${serviceIndex}`);
        const portIndex = portsContainer.children('.port').length;

        let portDiv = `<div class="port">
                                    <input type="number" name="Services[${serviceIndex}].Ports[${portIndex}].HostPort" placeholder="Host Port" class="form-control" />
                                    <input type="number" name="Services[${serviceIndex}].Ports[${portIndex}].ContainerPort" placeholder="Container Port" class="form-control" />
                                    <input type="text" name="Services[${serviceIndex}].Ports[${portIndex}].Protocol" placeholder="Protocol (default: tcp)" class="form-control" />
                                    <button type="button" class="remove-port-btn btn btn-danger">Remove Port</button>
                                </div>`;
        portsContainer.append(portDiv);
    });


    $('#services-container').on('click', '.remove-service', function () {
        $(this).closest('.service-item').remove();
    });


    // Add Environment Variable
    $('#services-container').on('click', '.add-env-btn', function () {
        const serviceIndex = $(this).data('service-index');
        const envContainer = $(`#environment-container-${serviceIndex}`);
        const envIndex = envContainer.children('.environment').length;

        let envDiv = `<div class="environment">
                                  <input type="text" name="Services[${serviceIndex}].Environment[${envIndex}].Key" placeholder="Key" class="form-control" />
                                  <input type="text" name="Services[${serviceIndex}].Environment[${envIndex}].Value" placeholder="Value" class="form-control" />
                                  <button type="button" class="remove-env-btn btn btn-danger">Remove Environment Variable</button>
                              </div>`;
        envContainer.append(envDiv);
    });

    // Add Network
    $('#services-container').on('click', '.add-network-btn', function () {
        const serviceIndex = $(this).data('service-index');
        const networksContainer = $(`#networks-container-${serviceIndex}`);
        const networkIndex = networksContainer.children('.network').length;

        let networkDiv = `<div class="network">
                                      <input type="text" name="Services[${serviceIndex}].Networks[${networkIndex}]" placeholder="Network Name" class="form-control" />
                                      <button type="button" class="remove-network-btn btn btn-danger">Remove Network</button>
                                  </div>`;
        networksContainer.append(networkDiv);
    });

    // Remove Port
    $('#services-container').on('click', '.remove-port-btn', function () {
        $(this).closest('.port').remove();
    });

    // Remove Volume
    $('#services-container').on('click', '.remove-volume-btn', function () {
        $(this).closest('.volume').remove();
    });

    // Remove Environment Variable
    $('#services-container').on('click', '.remove-env-btn', function () {
        $(this).closest('.environment').remove();
    });

    // Remove Network
    $('#services-container').on('click', '.remove-network-btn', function () {
        $(this).closest('.network').remove();
    });
    // Add Network
    $('#add-network').on('click', function () {
        let networkIndex = $('#networks-container .network-item').length;
        let newNetwork = `<div class="network-item" data-index="${networkIndex}">
                                            <h4>Network ${networkIndex}</h4>
                                            <div>
                                                <label for="Networks_${networkIndex}__Name">Name</label>
                                                <input id="Networks_${networkIndex}__Name" name="Networks[${networkIndex}].Name" class="form-control" />
                                            </div>
                                            <button type="button" class="remove-network btn btn-danger">Remove Network</button>
                                          </div>`;
        $('#networks-container').append(newNetwork);
    });

    // Remove Network
    $('#networks-container').on('click', '.remove-network', function () {
        $(this).closest('.network-item').remove();
    });

   
    // Remove Volume
    $('#volumes-container').on('click', '.remove-volume', function () {
        $(this).closest('.volume-item').remove();
    });

});