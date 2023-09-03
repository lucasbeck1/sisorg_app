import React, { useState } from 'react';
import axios from 'axios';

function FileUpload() {
  const [selectedFile, setSelectedFile] = useState(null);
  const [uploadStatus, setUploadStatus] = useState('');
  const [countries, setCountries] = useState([]);

  const countriesInfo = countries.length > 0;

  const handleFileChange = (event) => {
    setSelectedFile(event.target.files[0]);
  };

  const handleUpload = async () => {
    if (!selectedFile) {
      setUploadStatus('Select a file before uploading it');
      return;
    }

    const formData = new FormData();
    formData.append('file', selectedFile);

    try {
      setUploadStatus('Loading. please wait ...');
      const response = await axios.post('http://localhost:5202/api/File/upload', formData);
      
      if(response.statusText === "OK"){
        setCountries(response.data.rows);
        setUploadStatus('File uploaded successfully');
      }else{
        setUploadStatus('Error uploading file');
      }

      console.log('Server response:', response);
    } catch (error) {

      if(typeof(error.response.data) === "string"){
        setUploadStatus(error.response.data);
      }else{
        setUploadStatus('Error uploading file');
      }

      console.error('Error:', error);
    }
  };

  return (
    <div>
      <input type="file" onChange={handleFileChange} />
      <button onClick={handleUpload}>Upload</button>
      <p>{uploadStatus}</p>
      <section className='table'>
      {
      (countriesInfo) &&
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Value</th>
            <th>Color</th>
          </tr>
        </thead>
        <tbody>
        {countries.map((country, index) => 
            <tr key={index}>
              <td>{country.name}</td>
              <td>{country.value}</td>
              <td>{country.color}</td>
          </tr>
        )}
        </tbody>
      </table>
      }
      </section>
    </div>
  );
}

export default FileUpload;
