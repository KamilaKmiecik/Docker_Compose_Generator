// Dodawanie dynamicznych elementów (usługi, sieci, wolumeny)

function addDynamicElement(containerId, endpoint, type) {
    const container = document.getElementById(containerId);
    const index = container.children.length;

    fetch(`${endpoint}?index=${index}`)
        .then(response => response.text())
        .then(html => {
            const div = document.createElement('div');
            div.classList.add(`${type.toLowerCase()}-item`);
            div.innerHTML = html;

            container.appendChild(div);
            div.scrollIntoView({ behavior: 'smooth', block: 'start' });
           
           // const removeButton = document.getElementById(`${type.toLowerCase()}-rem-${index}`);
           // removeButton.addEventListener('click', () => container.removeChild(div));


            const removeButton = document.getElementById(`${type.toLowerCase()}-rem-${index}`);
            removeButton.addEventListener('click', () => {
                const previousElement = div.previousElementSibling; // Znajdź poprzedni element
                container.removeChild(div); // Usuń obecny element

                // Jeśli istnieje poprzedni element, przewiń do niego
                if (previousElement) {
                    previousElement.scrollIntoView({ behavior: 'smooth', block: 'start' });
                }
            });
        })
        .catch(error => console.error(`Error adding ${type}:`, error));
}

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
    } else if (target.classList.contains('remove-driver-option-button')) {
        target.closest('.driver-option-item').remove();
    }

    // Obsługa opcji sterownika w wolumenach
    document.addEventListener('click', function (event) {
        const target = event.target;

        if (target.classList.contains('add-driver-option-button')) {
            const index = target.getAttribute('data-volume-index');
            const container = document.getElementById(`driver-options-container-${index}`);

            if (!container) {
                console.error(`Container for driver options not found: driver-options-container-${index}`);
                return;
            }

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
            const driverOptionItem = target.closest('.driver-option-item');
            if (driverOptionItem) {
                driverOptionItem.remove();
            } else {
               // console.error('Driver option item to remove not found.');
            }
        }
    });


    // Obsługa etykiet (Labels) w wolumenach
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
    } else if (target.classList.contains('remove-label-button')) {
        target.closest('.label-item').remove();
    }


    //IPAM
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

    // Obsługa opcji sterownika w sieciach
    if (target.classList.contains('add-driver-option-button')) {
        const index = target.getAttribute('data-network-index');
        const container = document.getElementById(`driver-options-${index}`);
        const driverOptionHtml = `
            <div class="driver-option-item">
                <label>Key:</label>
                <input type="text" name="Networks[${index}].DriverOptions[newKey]" placeholder="Enter key" />
                <label>Value:</label>
                <input type="text" name="Networks[${index}].DriverOptions[newValue]" placeholder="Enter value" />
                <button type="button" class="remove-driver-option-button">Remove Option</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', driverOptionHtml);
    } else if (target.classList.contains('remove-driver-option-button')) {
        target.closest('.driver-option-item').remove();
    }


    // Obsługa opcji sterownika
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
    } else if (target.classList.contains('remove-driver-option-button')) {
        target.closest('.driver-option-item').remove();
    }

    // Obsługa zmiennych środowiskowych
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
    } else if (target.classList.contains('remove-env-button')) {
        target.closest('.env-item').remove();
    }

    // Obsługa portów
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
    } else if (target.classList.contains('remove-port-button')) {
        target.closest('.port-item').remove();
    }

    // Obsługa wolumenów
    if (target.classList.contains('add-volume-button')) {
        const index = target.getAttribute('data-service-index');
        const container = document.getElementById(`volume-container-${index}`);
        const volumeIndex = container.children.length;

        const volumeHtml = `
            <div class="volume-item">
                <label>Name:</label>
                <input type="text" name="Services[${index}].Volumes[${volumeIndex}].Name" placeholder="Name" />
                <label>Source:</label>
                <input type="text" name="Services[${index}].Volumes[${volumeIndex}].Source" placeholder="Source" />
                <label>Target:</label>
                <input type="text" name="Services[${index}].Volumes[${volumeIndex}].Target" placeholder="Target" />
                <button type="button" class="remove-volume-button">Remove</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', volumeHtml);
    } else if (target.classList.contains('remove-volume-button')) {
        target.closest('.volume-item').remove();
    }

    // Obsługa sieci
    if (target.classList.contains('add-network-button')) {
        const serviceIndex = target.getAttribute('data-service-index');
        const container = document.querySelector(`#network-container-${serviceIndex}`);
        const networkIndex = container.children.length;

        const networkHtml = `
            <div class="network-item">
                <label>Name:</label>
                <input type="text" name="Services[${serviceIndex}].Networks[${networkIndex}].Name" placeholder="Network Name" />
                <button type="button" class="remove-network-button">Remove</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', networkHtml);
    } else if (target.classList.contains('remove-network-button')) {
        target.closest('.network-item').remove();
    }
});

// Dodawanie głównych dynamicznych elementów
document.getElementById('add-service').addEventListener('click', () => {
    addDynamicElement('services-container', '/DockerCompose/GetServicePartial', 'Service');
});

document.getElementById('add-network').addEventListener('click', () => {
    addDynamicElement('networks-container', '/DockerCompose/GetNetworkPartial', 'Network');
});

document.getElementById('add-volume').addEventListener('click', () => {
    addDynamicElement('volumes-container', '/DockerCompose/GetVolumePartial', 'Volume');
});



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