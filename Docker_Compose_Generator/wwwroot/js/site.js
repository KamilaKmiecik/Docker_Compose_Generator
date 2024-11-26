document.getElementById('add-service').addEventListener('click', function () {
    const container = document.getElementById('services-container');
    const index = container.children.length;

    fetch(`/DockerCompose/GetServicePartial?index=${index}`)
        .then(response => response.text())
        .then(html => {
            const div = document.createElement('div');
            div.innerHTML = html;
            container.appendChild(div);

            div.scrollIntoView({ behavior: 'smooth', block: 'start' });
        });
});

document.getElementById('add-network').addEventListener('click', function () {
    const container = document.getElementById('networks-container');
    const index = container.children.length;

    fetch(`/DockerCompose/GetNetworkPartial?index=${index}`)
        .then(response => response.text())
        .then(html => {
            const div = document.createElement('div');
            div.innerHTML = html;
            container.appendChild(div);

            // Auto-scroll
            div.scrollIntoView({ behavior: 'smooth', block: 'start' });
        })
        .catch(error => console.error('Error adding network:', error));
});


document.getElementById('add-volume').addEventListener('click', function () {
    const container = document.getElementById('volumes-container');
    const index = container.children.length;

    fetch(`/DockerCompose/GetVolumePartial?index=${index}`)
        .then(response => response.text())
        .then(html => {
            const div = document.createElement('div');
            div.innerHTML = html;
            container.appendChild(div);

            div.scrollIntoView({ behavior: 'smooth', block: 'start' });
        });

});



//document.getElementById('add-service').addEventListener('click', function () {
//    addDynamicElement('services-container', '/DockerCompose/GetServicePartial', 'Service');
//});

//document.getElementById('add-network').addEventListener('click', function () {
//    addDynamicElement('networks-container', '/DockerCompose/GetNetworkPartial', 'Network');
//});

//document.getElementById('add-volume').addEventListener('click', function () {
//    addDynamicElement('volumes-container', '/DockerCompose/GetVolumePartial', 'Volume');
//});

function addDynamicElement(containerId, endpoint, type) {
    const container = document.getElementById(containerId);
    const index = container.children.length;

    fetch(`${endpoint}?index=${index}`)
        .then(response => response.text())
        .then(html => {
            const div = document.createElement('div');
            div.classList.add(`${type.toLowerCase()}-item`);
            div.innerHTML = html;

            const removeButton = document.createElement('button');
            removeButton.textContent = `Remove ${type}`;
            removeButton.classList.add('remove-button');
            removeButton.type = 'button';
            removeButton.addEventListener('click', function () {
                container.removeChild(div);
            });

            div.appendChild(removeButton);
            container.appendChild(div);
        })
        .catch(error => console.error(`Error adding ${type}:`, error));

}

document.addEventListener('click', function (event) {
    const target = event.target;


    if (target.classList.contains('add-ipam-button')) {
        const index = target.getAttribute('data-network-index');
        const container = document.getElementById(`ipam-configurations-${index}`);
        const ipamIndex = container.children.length;

        const ipamHtml = `
         <label>Driver:</label>
            <select name="Networks[${index}].Ipam.Driver">
                <option value=""> </option>
                <option value="bridge"> Bridge</option>
                <option value="host">Host</option>
                <option value="overlay" >Overlay</option>
            </select>
            <div class="ipam-item card">
                <h4>IPAM Configuration #${ipamIndex + 1}</h4>
                <div class="form-group">
                    <label>Subnet:</label>
                    <div class="ip-input">
                        <input type="number" min="0" max="255" name="Networks[${index}].Ipam.Configuration.Subnet[0]" placeholder="0" />
                        <span>.</span>
                        <input type="number" min="0" max="255" name="Networks[${index}].Ipam.Configuration.Subnet[1]" placeholder="0" />
                        <span>.</span>
                        <input type="number" min="0" max="255" name="Networks[${index}].Ipam.Configuration.Subnet[2]" placeholder="0" />
                        <span>.</span>
                        <input type="number" min="0" max="255" name="Networks[${index}].Ipam.Configuration.Subnet[3]" placeholder="0" />
                    </div>
                </div>
                <div class="form-group">
                    <label>Gateway:</label>
                    <div class="ip-input">
                        <input type="number" min="0" max="255" name="Networks[${index}].Ipam.Configuration.Gateway[0]" placeholder="0" />
                        <span>.</span>
                        <input type="number" min="0" max="255" name="Networks[${index}].Ipam.Configuration.Gateway[1]" placeholder="0" />
                        <span>.</span>
                        <input type="number" min="0" max="255" name="Networks[${index}].Ipam.Configuration.Gateway[2]" placeholder="0" />
                        <span>.</span>
                        <input type="number" min="0" max="255" name="Networks[${index}].Ipam.Configuration.Gateway[3]" placeholder="0" />
                    </div>
                </div>
                <button type="button" class="remove-ipam-button">Remove IPAM</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', ipamHtml);
    }

    if (target.classList.contains('remove-ipam-button')) {
        target.closest('.ipam-item').remove();
    }

    if (target.classList.contains('add-driver-option-button')) {
        const index = target.getAttribute('data-network-index');
        const container = document.getElementById(`driver-options-${index}`);
        const driverOptionHtml = `
            <div class="driver-option-item">
                <label>Key:</label>
                <input type="text" name="Networks[${index}].DriverOptions[newKey]" />

                <label>Value:</label>
                <input type="text" name="Networks[${index}].DriverOptions[newValue]" />

                <button type="button" class="remove-driver-option-button">Remove Option</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', driverOptionHtml);
    }

    if (target.classList.contains('remove-driver-option-button')) {
        target.closest('.driver-option-item').remove();
    }
});


document.addEventListener('click', function (event) {
    const target = event.target;

    if (target.classList.contains('add-env-button')) {
        const index = target.getAttribute('data-service-index');
        const container = document.getElementById(`env-container-${index}`);
        const envIndex = container.children.length;

        const envHtml = `
            <div class="env-item">
                <label>Key:</label>
                <input type="text" name="Services[${index}].Environment[${envIndex}].Key" placeholder="Key" />
                <label>Value:</label>
                <input type="text" name="Services[${index}].Environment[${envIndex}].Value" placeholder="Value" />
                <button type="button" class="remove-env-button">Remove</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', envHtml);
    }

    if (target.classList.contains('remove-env-button')) {
        target.closest('.env-item').remove();
    }

    // Ports
    if (target.classList.contains('add-port-button')) {
        const index = target.getAttribute('data-service-index');
        const container = document.getElementById(`port-container-${index}`);
        const portIndex = container.children.length;

        const portHtml = `
            <div class="port-item">
                <label>Host Port:</label>
                <input type="number" name="Services[${index}].Ports[${portIndex}].HostPort" placeholder="Host Port" />
                <label>Container Port:</label>
                <input type="number" name="Services[${index}].Ports[${portIndex}].ContainerPort" placeholder="Container Port" />
                <label>Protocol:</label>
                <select name="Services[${index}].Ports[${portIndex}].Protocol">
                    <option value=""></option>
                    <option value="tcp">TCP</option>
                    <option value="udp">UDP</option>
                </select>
                <button type="button" class="remove-port-button">Remove</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', portHtml);
    }

    if (target.classList.contains('remove-port-button')) {
        target.closest('.port-item').remove();
    }

    // Volumes
    if (target.classList.contains('add-volume-button')) {
        const index = target.getAttribute('data-service-index');
        const container = document.getElementById(`volume-container-${index}`);
        const volumeIndex = container.children.length;

        const volumeHtml = `
            <div class="volume-item">
                <label>Name/Type:</label>
                <input type="text" name="Services[${index}].Volumes[${volumeIndex}].Name" placeholder="Name/Type" />               
                <label>Source:</label>
                <input type="text" name="Services[${index}].Volumes[${volumeIndex}].Source" placeholder="Source" />
                <label>Target:</label>
                <input type="text" name="Services[${index}].Volumes[${volumeIndex}].Target" placeholder="Target" />
                <button type="button" class="remove-volume-button">Remove</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', volumeHtml);
    }

    if (target.classList.contains('remove-volume-button')) {
        target.closest('.volume-item').remove();
    }

    document.body.addEventListener('click', function (event) {
        if (event.target.classList.contains('add-network-button')) {
            const serviceIndex = event.target.getAttribute('data-service-index');
            const container = document.querySelector(`#network-container-${serviceIndex}`);
            const networkIndex = container.children.length;
            if (container) {
                // Dodaj nowy element sieci
                const networkItem = document.createElement('div');
                networkItem.classList.add('network-item');
                networkItem.innerHTML = `
                    <label>Name:</label>
                    <input type="text" name="Services[${serviceIndex}].Networks[${networkIndex}].Name" placeholder="Network Name" />
                    <button type="button" class="remove-network-button">Remove</button>
                `;
                container.appendChild(networkItem);
            }
        }
    });

    document.body.addEventListener('click', function (event) {
        if (event.target.classList.contains('remove-network-button')) {
            const networkItem = event.target.closest('.network-item');
            if (networkItem) {
                networkItem.remove();
            }
        }
});

//document.addEventListener('click', function (event) {
//    const target = event.target;

//    // Driver Options
//    if (target.classList.contains('add-driver-option-button')) {
//        const index = target.getAttribute('data-volume-index');
//        const container = document.getElementById(`driver-options-container-${index}`);
//        const driverOptionHtml = `
//            <div class="driver-option-item">
//                <label>Key:</label>
//                <input type="text" name="Volumes[${index}].DriverOptions[newKey]" placeholder="Enter key" />
//                <label>Value:</label>
//                <input type="text" name="Volumes[${index}].DriverOptions[newValue]" placeholder="Enter value" />
//                <button type="button" class="remove-driver-option-button">Remove Option</button>
//            </div>
//        `;
//        container.insertAdjacentHTML('beforeend', driverOptionHtml);
//    }

//    if (target.classList.contains('remove-driver-option-button')) {
//        target.closest('.driver-option-item').remove();
//    }

//    // Labels
//    if (target.classList.contains('add-label-button')) {
//        const index = target.getAttribute('data-volume-index');
//        const container = document.getElementById(`labels-container-${index}`);
//        const labelHtml = `
//            <div class="label-item">
//                <label>Key:</label>
//                <input type="text" name="Volumes[${index}].Labels[newKey]" placeholder="Enter key" />
//                <label>Value:</label>
//                <input type="text" name="Volumes[${index}].Labels[newValue]" placeholder="Enter value" />
//                <button type="button" class="remove-label-button">Remove Label</button>
//            </div>
//        `;
//        container.insertAdjacentHTML('beforeend', labelHtml);
//    }

//    if (target.classList.contains('remove-label-button')) {
//        target.closest('.label-item').remove();
//    }
//});


document.addEventListener('click', function (event) {
    const target = event.target;

    if (target.classList.contains('add-driver-option-button')) {
        const index = target.getAttribute('data-volume-index');
        const container = document.getElementById(`driver-options-container-${index}`);
        const driverOptionHtml = `
            <div class="driver-option-item">
                <label>Key:</label>
                <input type="text" name="Volumes[${index}].DriverOptions[newKey]" placeholder="Enter key" />
                <label>Value:</label>
                <input type="text" name="Volumes[${index}].DriverOptions[newValue]" placeholder="Enter value" />
                <button type="button" class="remove-driver-option-button">Remove Option</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', driverOptionHtml);
    }

    if (target.classList.contains('remove-driver-option-button')) {
        target.closest('.driver-option-item').remove();
    }
});

    document.addEventListener('click', function (event) {
    if (target.classList.contains('add-label-button')) {
        const index = target.getAttribute('data-volume-index');
        const container = document.getElementById(`labels-container-${index}`);
        const labelHtml = `
            <div class="label-item">
                <label>Key:</label>
                <input type="text" name="Volumes[${index}].Labels[newKey]" placeholder="Enter key" />
                <label>Value:</label>
                <input type="text" name="Volumes[${index}].Labels[newValue]" placeholder="Enter value" />
                <button type="button" class="remove-label-button">Remove Label</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', labelHtml);
    }

    if (target.classList.contains('remove-label-button')) {
        target.closest('.label-item').remove();
    }
});





/////
function updateIndices(containerId, itemPrefix) {
    const container = document.getElementById(containerId);
    const items = container.children;

    for (let i = 0; i < items.length; i++) {
        const item = items[i];
        item.id = `${itemPrefix}-item-${i}`;
        const inputElements = item.querySelectorAll("input, select, textarea");
        inputElements.forEach(input => {
            const name = input.name;
            input.name = name.replace(/\[\d+\]/, `[${i}]`);
        });
    }
}
////
function removeVolume(index) {
    const container = document.getElementById("volumes-container");
    const item = document.getElementById(`volume-item-${index}`);
    if (item) {
        item.remove();
        updateIndices("volumes-container", "volume");
    }
}

function removeNetwork(index) {
    const container = document.getElementById("networks-container");
    const item = document.getElementById(`network-item-${index}`);
    if (item) {
        item.remove(); 
        updateIndices("networks-container", "network"); 
    }
}

function removeService(index) {
    const container = document.getElementById("services-container");
    const item = document.getElementById(`service-item-${index}`);
    if (item) {
        item.remove(); 

        updateIndices("services-container", "service");
    }
}

function updateIndices(containerId, itemPrefix) {
    const container = document.getElementById(containerId);
    const items = container.querySelectorAll(`[id^="${itemPrefix}-item-"]`);

    items.forEach((item, newIndex) => {
        item.id = `${itemPrefix}-item-${newIndex}`;

        const inputs = item.querySelectorAll("input, select, textarea");
        inputs.forEach(input => {
            if (input.name) {
                input.name = input.name.replace(/\[\d+\]/, `[${newIndex}]`);
            }
        });

        const removeButton = item.querySelector(".remove-button");
        if (removeButton) {
            removeButton.setAttribute("onclick", `removeService(${newIndex})`);
        }
    });
}

//function removeService(index) {
//    var id = document.getElementById(`service-item-${index}`)
//    const item = id;
//    if (item) {
//        item.remove(); // Usuń element DOM
//    }
//}


document.querySelector('form').addEventListener('submit', function (event) {
    const ipamItems = document.querySelectorAll('.ipam-item');

    ipamItems.forEach((item) => {
        const subnetInputs = item.querySelectorAll('input[name*="Subnet"]');
        const subnet = Array.from(subnetInputs).map(input => input.value).join('.');

        const subnetField = document.createElement('input');
        subnetField.type = 'hidden';
        subnetField.name = subnetInputs[0].name.replace(/\.Subnet\[\d+\]/, '.Subnet');
        subnetField.value = subnet;

        subnetInputs.forEach(input => input.remove());

        item.appendChild(subnetField);

        const gatewayInputs = item.querySelectorAll('input[name*="Gateway"]');
        const gateway = Array.from(gatewayInputs).map(input => input.value).join('.');

        const gatewayField = document.createElement('input');
        gatewayField.type = 'hidden';
        gatewayField.name = gatewayInputs[0].name.replace(/\.Gateway\[\d+\]/, '.Gateway');
        gatewayField.value = gateway;

        gatewayInputs.forEach(input => input.remove());

        item.appendChild(gatewayField);
    });
});

///

});
