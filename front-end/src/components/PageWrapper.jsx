import React from 'react';

const PageWrapper = ({ children }) => {
  return (
    <div className="py-20 bg-background-custom min-h-screen">
        {children}
    </div>

  );
};

export default PageWrapper;
