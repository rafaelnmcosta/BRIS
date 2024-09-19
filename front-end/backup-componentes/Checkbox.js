import React, { useState } from 'react';
import '../App.css';

function Checkbox( { texto } ) {
  const [isChecked, setIsChecked] = useState(false);

  const handleCheckboxChange = () => {
    setIsChecked(!isChecked);
  };

  return (
    <div className='checkbox'>
      <input
        type="checkbox"
        checked={isChecked}
        onChange={handleCheckboxChange}
      />
      <label>{texto}</label>
    </div>
  );
}

export default Checkbox;
