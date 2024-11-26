import React from 'react';

const PageWrapper = ({ children }) => {
  return (
    <div className="pt-16 px-16 w-full min-h-full flex flex-col bg-background-custom">
        {children}
    </div>

  );
};

export default PageWrapper;
