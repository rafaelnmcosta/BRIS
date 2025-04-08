import React from 'react';
import { Input } from 'antd';

const InputSemBordaComLabel = ({ 
  label, 
  name,
  value, 
  onChange, 
  placeholder, 
  icone, 
  suffix, 
  type = 'text',
  error,
  rows = 4
}) => {
  const handleChange = (e) => {
    let inputValue = '';
    
    // Trata diferentes formatos de eventos (React vs Ant Design)
    if (e && e.target) { // Evento nativo do React
      inputValue = e.target.value;
    } else if (typeof e === 'object' && e !== null) { // Objeto do Ant Design
      inputValue = e.target?.value || '';
    } else { // Valor direto (string/number)
      inputValue = e || '';
    }

    if (typeof onChange === 'function') {
      onChange({
        target: {
          name,
          value: inputValue
        }
      });
    }
  };

  const commonProps = {
    name,
    value: value || '',
    onChange: (e) => handleChange(e), // Garante formato consistente
    placeholder,
    prefix: icone,
    suffix,
    className: "border-b-2 border-t-0 border-x-0 border-green-dark hover:border-green focus:border-green focus-within:border-green"
  };

  switch (type) {
    case 'password':
      return (
        <div className="flex flex-col mb-4">
          <label className="mb-2 text-sm font-medium text-green-dark">{label}</label>
          <Input.Password
            {...commonProps}
          />
          {error && <span className="text-red-500 text-xs mt-1">{error}</span>}
        </div>
      );

    case 'textarea':
      return (
        <div className="flex flex-col mb-4">
          <label className="mb-2 text-sm font-medium text-green-dark">{label}</label>
          <Input.TextArea
            {...commonProps}
            rows={rows}
            autoSize={{ minRows: rows, maxRows: 6 }}
          />
          {error && <span className="text-red-500 text-xs mt-1">{error}</span>}
        </div>
      );

    case 'number':
      return (
        <div className="flex flex-col mb-4">
          <label className="mb-2 text-sm font-medium text-green-dark">{label}</label>
          <Input
            {...commonProps}
            type="number"
          />
          {error && <span className="text-red-500 text-xs mt-1">{error}</span>}
        </div>
      );

    default:
      return (
        <div className="flex flex-col mb-4">
          <label className="mb-2 text-sm font-medium text-green-dark">{label}</label>
          <Input
            {...commonProps}
            type={type}
          />
          {error && <span className="text-red-500 text-xs mt-1">{error}</span>}
        </div>
      );
  }
};

export default InputSemBordaComLabel;