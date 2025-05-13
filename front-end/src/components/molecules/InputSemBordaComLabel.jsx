import React from 'react';
import { Input } from 'antd';
import InputMask from 'react-input-mask';
import PasswordStrengthMeter from './PasswordStrengthMeter';

const InputSemBordaComLabel = ({
  label,
  name,
  value,
  onChange,
  placeholder,
  icone,
  suffix,
  type = 'text',
  error,  // mensagem de erro
  rows = 4,
  mask
}) => {
  const handleChange = (e) => {
    let inputValue = '';

    if (e && e.target) {
      inputValue = e.target.value;
    } else {
      inputValue = e || '';
    }

    if (typeof onChange === 'function') {
      onChange({ target: { name, value: inputValue } });
    }
  };

  // Borda vermelha se erro, verde caso contrário
  const borderClass = error
    ? 'border-red-500 focus:border-red-500 hover:border-red-500'
    : 'border-green-dark hover:border-green focus:border-green focus-within:border-green';

  const commonProps = {
    name,
    value: value || '',
    onChange: (e) => handleChange(e),
    placeholder,
    prefix: icone,
    suffix,
    className: `border-b-2 border-t-0 border-x-0 ${borderClass}`
  };

  const renderInput = () => {
    if (mask) {
      const maxDigitsFromMask = (mask) => (mask.match(/9/g) || []).length;

      const handleMaskChange = (e) => {
        const limite = maxDigitsFromMask(mask);
        let apenasDigitos = e.target.value.replace(/\D/g, '');
        if (apenasDigitos.length > limite) {
          apenasDigitos = apenasDigitos.slice(0, limite);
        }
        handleChange({ target: { name, value: apenasDigitos } });
      };

      return (
        <InputMask
          mask={mask}
          value={value || ''}
          onChange={handleMaskChange}
          maskChar={null}
        >
          {(inputProps) => (
            <Input
              {...commonProps}
              {...inputProps}
              onChange={undefined}
            />
          )}
        </InputMask>
      );
    }
    return <Input {...commonProps} type={type} />;
  };

  const renderField = () => {
    switch (type) {
      case 'passwordCadastro':
        return (
          <>
            <Input.Password {...commonProps} />
            <PasswordStrengthMeter senha={value} />
          </>
        );
      case 'password':
        return <Input.Password {...commonProps} />;
      case 'textarea':
        return (
          <Input.TextArea
            {...commonProps}
            rows={rows}
            autoSize={{ minRows: rows, maxRows: 6 }}
          />
        );
      case 'number':
        return <Input {...commonProps} type="number" />;
      default:
        return renderInput();
    }
  };

  return (
    <div className="flex flex-col mb-4">
      <label className="mb-2 text-sm font-medium text-green-dark">{label}</label>
      {renderField()}
      {error && <span className="text-red-500 text-xs mt-1">{error}</span>}
    </div>
  );
};

export default InputSemBordaComLabel;