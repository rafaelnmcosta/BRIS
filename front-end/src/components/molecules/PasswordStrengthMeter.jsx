import React from 'react';
import zxcvbn from 'zxcvbn';

const PasswordStrengthMeter = ({ senha }) => {
  const { score } = zxcvbn(senha);

  const cores = ['bg-red-500', 'bg-orange-500', 'bg-yellow-400', 'bg-green-500', 'bg-green-700'];
  const textos = ['Muito fraca', 'Fraca', 'Razoável', 'Forte', 'Muito forte'];

  return (
    <div className="mt-2">
      <div className="h-2 rounded bg-gray-200">
        <div className={`h-full ${cores[score]} transition-all duration-300`} style={{ width: `${(score + 1) * 20}%` }} />
      </div>
      <p className="text-sm mt-1 text-gray-700">{textos[score]}</p>
      {/*feedback.warning && <p className="text-sm text-yellow-600">{feedback.warning}</p>*/}
    </div>
  );
};

export default PasswordStrengthMeter;